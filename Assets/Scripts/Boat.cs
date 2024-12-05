using System;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int scoreIncrementOnFlip = 5;
    
    [Header("Boat Behavior")]
    [SerializeField] private float uprightStabilizationForce = 10f;
    [SerializeField] private float stabilizationDamping = 2f;
    
    public bool isFlipped;
    private Health _health;
    private Vector3 _velocity;
    private bool _isKnockedBack;
    private float _knockbackDuration = 0.2f;
    private float _knockbackTimer;
    private Rigidbody _rigidbody;
    private float _tiltAngle;
    private float _depth;

    public bool isInWater;
    private bool _stopUpdate;
    
    [SerializeField] private float tiltThreshold;
    [SerializeField] private float maxTilt;
    
    private void Awake()
    {
        if (!TryGetComponent(out _health))
        {
            Debug.LogError("Health Component Not Found.");
        }
        
        if (!TryGetComponent(out _rigidbody))
        {
            Debug.LogError("Rigid Body Component Not Found.");
        }
    }
    
    
    private void FixedUpdate()
    {
        if (_stopUpdate)
        {
            return;
        }
        
        if (_isKnockedBack) 
        {
            _knockbackTimer -= Time.deltaTime;
            if (_knockbackTimer <= 0) 
            {
                _isKnockedBack = false;
                _velocity = Vector3.zero;
            }
        }
        
        _rigidbody.AddForce(_velocity, ForceMode.Impulse);
        _rigidbody.AddTorque(_velocity*100, ForceMode.Force);
        
        _tiltAngle = Vector3.Angle(transform.up, Vector3.up);
        _depth = GameManager.Instance.WaterLevel - _rigidbody.transform.position.y;

        if (_tiltAngle > maxTilt || _depth > -2.0f)
        {
            Sink();
        }
        StabilizeUpright();
    }

    
    void Update()
    {
        _health.HealthBar.gameObject.transform.LookAt(GameManager.Instance.PlayerController.transform);
        
        if (_stopUpdate)
        {
            return;
        }
        
        _rigidbody.useGravity = !isInWater;
        if (_rigidbody.useGravity)
        {
            _rigidbody.AddForceAtPosition(Vector3.down * 10000.0f, _rigidbody.position);
        }

        if (_health.IsDead)
        {
            Sink();
        }
    }

    
    public void ApplyKnockback(Vector3 direction, float attackPower, int damage) 
    {
        _isKnockedBack = true;
        _knockbackTimer = _knockbackDuration;
        _velocity = direction.normalized * attackPower;
      
        _health.TakeDamage(damage);
    }
    
    
    void StabilizeUpright()
    {
        if (_tiltAngle > tiltThreshold && _tiltAngle < maxTilt)
        {
            float stabilizationStrength = Mathf.InverseLerp(tiltThreshold, maxTilt, _tiltAngle) * uprightStabilizationForce;

            Vector3 torque = Vector3.Cross(transform.up, Vector3.up) * stabilizationStrength;

            _rigidbody.AddTorque(torque - _rigidbody.angularVelocity * stabilizationDamping, ForceMode.Force);
        }
    }

    private void Sink()
    {
        ScoreManager.Instance.AddScore(scoreIncrementOnFlip);
        _stopUpdate = true;
        isFlipped = true;
        _rigidbody.useGravity = true;
        _rigidbody.mass = 10000;
        _health.HealthBar.gameObject.SetActive(false);
    }
    /*private int getMassDecrementStep()
    {
        int damage = GameManager.Instance.PlayerController.Damage;
        float health = _health.MaxHealth;
        float rbMass = _rigidbody.mass;

        int steps = (int)health / damage;
        int mass = (int) (rbMass - _actualMass);
        
        return mass/steps;
    }*/
}
