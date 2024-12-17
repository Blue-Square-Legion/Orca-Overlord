using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Boat : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int scoreIncrementOnFlip = 5;

    [Header("Boat Behavior")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float escapePositionOffset;
    [SerializeField] private float playerDetectionDistance;
    [SerializeField] private float uprightStabilizationForce = 10f;
    [SerializeField] private float stabilizationDamping = 2f;
    [SerializeField] private float tiltThreshold;
    [SerializeField] private float maxTilt;

    [Header("Wave Interaction")]
    [SerializeField] private float waveFrequency = 1f; 
    [SerializeField] private float waveSpeed = 1f;
    [SerializeField] private float waveHeight = 2f;
    [SerializeField] private float buoyancyFactor = 10f;

    private Health _health;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _velocity;
    private Vector3 _position;
    private bool _isKnockedBack;
    private float _knockBackDuration = 0.2f;
    private float _knockBackTimer;
    private Rigidbody _rigidBody;
    private float _tiltAngle;
    private float _depth;
    private bool _stopUpdate;
    private bool _isMovingAway;
    private float _distanceFromPlayer;
    private PlayerController _playerController;

    [SerializeField] private MeshFilter waveMesh;
    
    [HideInInspector] public bool isFlipped;
    [HideInInspector] public bool isInWater;

    private void Awake()
    {
        if (!TryGetComponent(out _health))
        {
            Debug.LogError("Health Component Not Found.");
        }

        if (!TryGetComponent(out _rigidBody))
        {
            Debug.LogError("Rigid Body Component Not Found.");
        }

        if (!TryGetComponent(out _navMeshAgent))
        {
            Debug.LogError("NavMesh Agent Component Not Found.");
        }
    }

    private void Start()
    {
        _playerController = GameManager.Instance.PlayerController;
    }

    private void FixedUpdate()
    {
        if (_stopUpdate)
        {
            return;
        }
        
        _distanceFromPlayer = Vector3.Distance(transform.position, _playerController.transform.position);

        float distanceFromPlayer = Vector3.Distance(transform.position, _playerController.transform.position);

        if (distanceFromPlayer < playerDetectionDistance)
        {
            _isMovingAway = true;
            MoveAwayFromPlayer();
        }
        else
        {
            _isMovingAway = false;
            _navMeshAgent.ResetPath();
            _rigidBody.velocity = Vector3.zero;
        }

        if (_isKnockedBack)
        {
            _knockBackTimer -= Time.deltaTime;
            if (_knockBackTimer <= 0)
            {
                _isKnockedBack = false;
                _velocity = Vector3.zero;
            }
        }

        _rigidBody.AddForce(_velocity, ForceMode.Impulse);
        _rigidBody.AddTorque(_velocity * 100, ForceMode.Force);

        _tiltAngle = Vector3.Angle(transform.up, Vector3.up);
        _depth = GameManager.Instance.WaterLevel - _rigidBody.transform.position.y;

        if (_tiltAngle > maxTilt || _depth > 15)
        {
            Sink();
        }

        ApplyWaveForces();
        StabilizeUpright();
    }

    void Update()
    {
        //PrintDebugInfo();
        
        _health.HealthBar.gameObject.transform.LookAt(GameManager.Instance.PlayerController.transform);

        if (_stopUpdate)
        {
            return;
        }

        _rigidBody.useGravity = !isInWater;

        if (_rigidBody.useGravity)
        {
            _rigidBody.AddForceAtPosition(Vector3.down * 10000.0f, _rigidBody.position);
        }

        if (_health.IsDead)
        {
            Sink();
        }
    }

    public void ApplyKnockBack(Vector3 direction, float attackPower, int damage)
    {
        _isKnockedBack = true;
        _knockBackTimer = _knockBackDuration;
        _velocity = direction.normalized * attackPower;

        _health.TakeDamage(damage);
    }

    void StabilizeUpright()
    {
        if (_tiltAngle > tiltThreshold && _tiltAngle < maxTilt)
        {
            float stabilizationStrength = Mathf.InverseLerp(tiltThreshold, maxTilt, _tiltAngle) * uprightStabilizationForce;

            Vector3 torque = Vector3.Cross(transform.up, Vector3.up) * stabilizationStrength;

            _rigidBody.AddTorque(torque - _rigidBody.angularVelocity * stabilizationDamping, ForceMode.Force);
        }
    }

    private void Sink()
    {
        ScoreManager.Instance.AddScore(scoreIncrementOnFlip);
        _stopUpdate = true;
        isFlipped = true;
        _rigidBody.useGravity = true;
        _rigidBody.mass = 10000;
        _health.HealthBar.gameObject.SetActive(false);
        _navMeshAgent.enabled = false;
    }

    // Apply buoyancy and wave forces based on the boat's position and the wave mesh
    void ApplyWaveForces()
    {
        if (!waveMesh)
        {
            return;
        }

        float waveHeightAtBoat = GetWaveHeight(transform.position);
        float displacement = waveHeightAtBoat - _rigidBody.position.y;
        
        if (displacement > 0)
        {
            Vector3 buoyantForce = Vector3.up * displacement * buoyancyFactor;
            _rigidBody.AddForce(buoyantForce, ForceMode.Force);
        }
        
        Vector3 waveSlope = GetWaveSlope(transform.position);
        Vector3 waveForce = waveSlope * buoyancyFactor;
        _rigidBody.AddForce(waveForce, ForceMode.Force);
    }

    
    
    float GetWaveHeight(Vector3 position)
    {
        Vector3 localPosition = waveMesh.GameObject().transform.InverseTransformPoint(position);

        float wave = Mathf.Sin(Time.time * waveSpeed + localPosition.x * waveFrequency) *
                     Mathf.Sin(Time.time * waveSpeed + localPosition.z * waveFrequency);

        return wave * waveHeight;
    }
    
    
    Vector3 GetWaveSlope(Vector3 position)
    {
        float delta = 0.1f;

        float heightX1 = GetWaveHeight(position + new Vector3(delta, 0, 0));
        float heightX2 = GetWaveHeight(position - new Vector3(delta, 0, 0));
        float heightZ1 = GetWaveHeight(position + new Vector3(0, 0, delta));
        float heightZ2 = GetWaveHeight(position - new Vector3(0, 0, delta));
        
        float slopeX = (heightX1 - heightX2) / (2 * delta);
        float slopeZ = (heightZ1 - heightZ2) / (2 * delta);

        return new Vector3(slopeX, 0, slopeZ).normalized;
    }
    
    
    
    private void MoveAwayFromPlayer()
    {
        // Calculate direction away from the player
        Vector3 directionAway = (transform.position - _playerController.transform.position).normalized;
        Vector3 targetPosition = transform.position + directionAway * 10f; // Arbitrary distance ahead

        // Set NavMeshAgent path to the target position
        _navMeshAgent.SetDestination(targetPosition);

        // Apply movement using Rigidbody
        Vector3 desiredVelocity = _navMeshAgent.desiredVelocity;
        Vector3 horizontalVelocity = new Vector3(desiredVelocity.x, 0, desiredVelocity.z);
        _rigidBody.AddForce(horizontalVelocity.normalized * moveSpeed, ForceMode.Acceleration);

        // Face the movement direction
        if (horizontalVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
            _rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f));
        }
    }

    void PrintDebugInfo()
    {
        Debug.Log("Tilt Angle - " + _tiltAngle);
        Debug.Log("Depth - " + _depth);
    }
}
