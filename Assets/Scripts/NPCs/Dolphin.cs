using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Dolphin : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float followDistanceThreshold;
    [SerializeField] private float closenessToPlayer;
    [SerializeField] private float lookAtDistanceThreshold;
    [SerializeField] private float knockbackPower;
    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private Transform mesh;
    
    [Header("Pod Settings")]
    [Tooltip("Minimum speed of the Dolphin.")]
    [SerializeField] private float minSpeed = 1.0f;
    
    [Tooltip("Maximum speed of the Dolphin.")]
    [SerializeField] private float maxSpeed = 5.0f;
    
    [Tooltip("Rotation speed for the Dolphin.")]
    [SerializeField] private float rotationSpeed = 4.0f;

    [Tooltip("Minimum distance from the neighbor in a pod.")]
    [SerializeField] private float neighborDistance = 5.0f;
    
    private DolphinSpawner _dolphinSpawner;
    private Vector3 _center;
    private Vector3 _averageHeading;
    private Vector3 _averagePosition;
    private bool _turn;
    private float _tempDist;
    
    
    private GameObject _player;
    private State _currentState;
    private State _idle;
    private State _lookAt;
    private State _follow;
    private State _attack;
    
    
    public bool IsAttacking;
    public string CurrentState;
    
    public float MoveSpeed => moveSpeed;
    
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (!_player)
        {
            Debug.LogError("Player Not Found!");
        }
        
        _dolphinSpawner = GameManager.Instance.DolphinSpawner;
    }

    private void Start()
    {
        _idle = new Idle(this.GameObject(), _player, neighborDistance, moveSpeed, rotationSpeed, _dolphinSpawner);
        _lookAt = new LookAt(this.GameObject(), _player, neighborDistance, moveSpeed, rotationSpeed,_dolphinSpawner);
        _follow = new Follow(this.GameObject(), _player, closenessToPlayer, moveSpeed, neighborDistance, rotationSpeed,_dolphinSpawner);
        _attack = new Attack(this.GameObject(), _player, knockbackPower, damage, attackRange, attackCooldown, closenessToPlayer, moveSpeed, neighborDistance, rotationSpeed, _dolphinSpawner);
        SwitchState(_idle);
        IsAttacking = false;
        
        _center = _dolphinSpawner.spawnPoint.transform.position;
    }

    private void Update()
    {
        _currentState?.Update();
        CurrentState = _currentState?.ToString();
        
        distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if(distanceFromPlayer <= attackRange)
        {
            SwitchState(_attack);
            IsAttacking = true;
        }
        else if (distanceFromPlayer < lookAtDistanceThreshold)
        {
            SwitchState(_follow);
            IsAttacking = false;
        }
        else if (distanceFromPlayer < followDistanceThreshold)
        {
            SwitchState(_lookAt);
            IsAttacking = false;
        }
        else
        {
            SwitchState(_idle);
        }
    }

    public void SwitchState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
