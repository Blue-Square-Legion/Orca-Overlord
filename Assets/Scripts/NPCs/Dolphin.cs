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
    
    [Tooltip("Minimum speed of the Dolphin.")]
    [SerializeField] private float minSpeed = 1.0f;
    
    [Tooltip("Maximum speed of the Dolphin.")]
    [SerializeField] private float maxSpeed = 5.0f;
    
    [Tooltip("Rotation speed for the Dolphin.")]
    [SerializeField] private float rotationSpeed = 4.0f;

    [Tooltip("Minimum distance from the neighbor in a pod.")]
    [SerializeField] private float neighborDistance = 5.0f;
    
    private DolphinSpawner _dolphinSpawner;
    private float _speed;
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
        _idle = new Idle();
        _lookAt = new LookAt(this.GameObject(), _player);
        _follow = new Follow(this.GameObject(), _player, closenessToPlayer, moveSpeed);
        _attack = new Attack(this.GameObject(), _player, knockbackPower, damage, attackRange, attackCooldown, closenessToPlayer, moveSpeed);
        SwitchState(_idle);
        IsAttacking = false;
        
        //_speed = Random.Range(minSpeed, maxSpeed);
        _center = _dolphinSpawner.spawnPoint.transform.position;
    }

    private void Update()
    {
        _currentState?.Update();
        
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
        

        if (!IsAttacking && _currentState == _idle)
        {
            _center = _dolphinSpawner.spawnPoint.transform.position;

            if (_dolphinSpawner)
            {
                _tempDist = Vector3.Distance(transform.position, _center);

                if (Vector3.Distance(transform.position, _center) >= _dolphinSpawner.effectiveRadius)
                {
                    _turn = true;
                }
                else
                {
                    _turn = false;
                }

                if (_turn)
                {
                    Vector3 direction = _center - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                        rotationSpeed * Time.deltaTime);

                    _speed = Random.Range(minSpeed, maxSpeed);
                }
                else
                {
                    if (Random.Range(0, 5) < 2)
                    {
                        ApplyRules();
                    }
                }

                transform.Translate(Time.deltaTime * _speed, 0, 0);
            }
        }
    }

    public void SwitchState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
    
    void ApplyRules()
    {
        GameObject[] pod = _dolphinSpawner.allDolphins;

        Vector3 vcenter =_center;
        Vector3 vavoid = _center;
        float gSpeed = 0.1f;

        Vector3 goalPosition = _dolphinSpawner.goalPosition;

        float distance;

        int groupSize = 0;

        foreach (GameObject dolphin in pod)
        {
            if (dolphin != this.gameObject)
            {
                distance = Vector3.Distance(dolphin.transform.position, transform.position);

                if (distance <= neighborDistance)
                {
                    vcenter += dolphin.transform.position;
                    groupSize++;

                    if (distance < 1.0f)
                    {
                        vavoid += (this.transform.position - dolphin.transform.position);
                    }

                    Dolphin anotherDolphin = dolphin.GetComponent<Dolphin>();
                    gSpeed += anotherDolphin._speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPosition - transform.position);
            _speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
