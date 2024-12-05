using System;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private int scoreIncrementOnFlip = 5;
    [SerializeField] private float _maxSubmergeDistance = 30;
    
    private bool _canBeFlipped;
    private bool _isFlipped;
    private Health _health;
    private Vector3 _velocity;
    private bool _isKnockedBack;
    private float _knockbackDuration = 0.2f;
    private float _knockbackTimer;
    private Rigidbody _rigidbody;
    private bool _isInWater;
    private bool _lastAttack;
    private bool _stopUpdate;
    private float _actualMass;
    private float _actualAngularDrag;
    private float _actualDrag;
    [SerializeField] private float maxTiltTolerance;
    private float _massDecrementStep;
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
        else
        {
            _actualMass = _rigidbody.mass;
            _actualDrag = _rigidbody.drag;
            _actualAngularDrag = _rigidbody.angularDrag;
            
            _rigidbody.mass = 10000.0f;
            _rigidbody.angularDrag = 10.0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _massDecrementStep = getMassDecrementStep();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _isInWater = true;
            _rigidbody.drag *= 5;
            _rigidbody.angularDrag *= 5;
        }
    }

    private void OnTriggerExit(Collider other)
    { 
        if (other.CompareTag("Water"))
        {
            _isInWater = false;
            _rigidbody.drag /= 5;
            _rigidbody.angularDrag /= 5;
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (_stopUpdate)
        {
            return;
        }
        
        _rigidbody.useGravity = !_isInWater;
        
        float tiltAngle = transform.rotation.eulerAngles.x;
        if (Mathf.Abs(tiltAngle) > 0 && Mathf.Abs(tiltAngle) < maxTiltTolerance)
        {
            Vector3 correctiveTorque = new Vector3(-tiltAngle, 0, 0);
            _rigidbody.AddTorque(correctiveTorque, ForceMode.Force);
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

        if (_canBeFlipped)
        {
            _rigidbody.mass = _actualMass;
            _rigidbody.angularDrag = _actualAngularDrag;
        }
        
        if (_isFlipped)
        {
            ScoreManager.Instance.AddScore(scoreIncrementOnFlip);
            _stopUpdate = true;
            _rigidbody.useGravity = true;
        }

        _rigidbody.AddForce(_velocity * Time.deltaTime, ForceMode.Impulse);
    }

    public void ApplyKnockback(Vector3 direction, float attackPower, int damage) 
    {
        _isKnockedBack = true;
        _knockbackTimer = _knockbackDuration;
        _velocity = direction * attackPower;
      
        _health.TakeDamage(damage);

        if (_rigidbody.mass > _actualMass)
        {
            _rigidbody.mass -= _massDecrementStep;
        }

        _canBeFlipped = _health.IsDead;
        _lastAttack = _canBeFlipped;
        _isFlipped = _lastAttack;
    }

    void SinkBoat()
    {
        // Gradually lower the boat’s position to simulate sinking
        _rigidbody.AddForce(Vector3.down * 5, ForceMode.Force);
    }
    
    private int getMassDecrementStep()
    {
        int damage = GameManager.Instance.PlayerController.Damage;
        float health = _health.MaxHealth;
        float rbMass = _rigidbody.mass;

        int steps = (int)health / damage;
        int mass = (int) (rbMass - _actualMass);
        
        return mass/steps;
    }
}
