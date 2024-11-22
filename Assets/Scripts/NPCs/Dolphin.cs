using Unity.VisualScripting;
using UnityEngine;

public class Dolphin : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float followDistanceThreshold;
    [SerializeField] private float closenessToPlayer;
    [SerializeField] private float lookAtDistanceThreshold;
    [SerializeField] private float attackPower;
    [SerializeField] private float timeBetweenAttacks;
    
    [SerializeField] private float _distanceFromPlayer;
    
    private GameObject _player;
    private State _currentState;
    private State _idle;
    private State _lookAt;
    private State _follow;
    private State _attack;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (!_player)
        {
            Debug.LogError("Player Not Found!");
        }
    }

    private void Start()
    {
        _idle = new Idle();
        _lookAt = new LookAt(this.GameObject(), _player);
        _follow = new Follow(this.GameObject(), _player, moveSpeed);
        _attack = new Attack(this.GameObject(), _player, attackPower, timeBetweenAttacks);
        
        SwitchState(_idle);
    }

    private void Update()
    {
        
        _currentState.Update();
        
        _distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);


        if (_currentState == _idle)
        {
            if (_distanceFromPlayer < lookAtDistanceThreshold)
            {
                SwitchState(_lookAt);
            }
        }

        if (_currentState == _lookAt)
        {
            if (_distanceFromPlayer > lookAtDistanceThreshold)
            {
                SwitchState(_idle);
            }
            
            if (_distanceFromPlayer < followDistanceThreshold && _distanceFromPlayer > closenessToPlayer)
            {
                SwitchState(_follow);
            }
        }

        if (_currentState == _follow)
        {
            if (_distanceFromPlayer > followDistanceThreshold)
            {
                SwitchState(_lookAt);
            }
            
            if(_distanceFromPlayer <= closenessToPlayer)
            {
                SwitchState(_attack);
            }
        }

        if (_currentState == _attack)
        {
            if (_distanceFromPlayer > closenessToPlayer)
            {
                SwitchState(_follow);
            }
        }
    }

    public void SwitchState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
