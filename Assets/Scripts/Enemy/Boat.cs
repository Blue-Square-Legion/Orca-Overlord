using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Boat : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private int scoreIncrementOnFlip = 5;

    [Header("Boat Behavior")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float playerDetectionDistance;
    [SerializeField] private float uprightStabilizationForce = 10f;
    [SerializeField] private float stabilizationDamping = 2f;
    [SerializeField] private float tiltThreshold;
    [SerializeField] private float maxTilt;

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

        if (!transform.parent.TryGetComponent(out _navMeshAgent))
        {
            Debug.LogError("NavMesh Agent Component Not Found.");
        }
    }

    private void Start()
    {
        _playerController = GameManager.Instance.PlayerController;
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    private void FixedUpdate()
    {
        if (_stopUpdate)
        {
            return;
        }
        
        _distanceFromPlayer = Vector3.Distance(transform.position, _playerController.transform.position);

        if (_distanceFromPlayer < playerDetectionDistance)
        {
            _isMovingAway = true;
            MoveAwayFromPlayer();
        }
        else
        {
            _isMovingAway = false;
            _navMeshAgent?.ResetPath();
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
    }

    void Update()
    {
        _health.HealthBar.gameObject.transform.LookAt(GameManager.Instance.PlayerController.transform);

        if (_stopUpdate)
        {
            return;
        }

        _navMeshAgent.nextPosition = _rigidBody.position;
        
        if (_navMeshAgent.velocity.magnitude > 0.1f)
        {
            Vector3 direction = _navMeshAgent.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rigidBody.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
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
    
    
    
    private void MoveAwayFromPlayer()
    {
        Vector3 directionAway = (transform.position - _playerController.transform.position).normalized;
        Vector3 targetPosition = transform.position + directionAway * 50f;

        _navMeshAgent.SetDestination(targetPosition);
        
        Vector3 desiredVelocity = _navMeshAgent.desiredVelocity;
        Vector3 horizontalVelocity = new Vector3(desiredVelocity.x, 0, desiredVelocity.z);
        _rigidBody.AddForce(horizontalVelocity.normalized * moveSpeed, ForceMode.Acceleration);

        if (horizontalVelocity == Vector3.zero)
        {
            return;
        }
        
        Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
        _rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f));
    }
}
