using UnityEngine;

public class Idle : State
{
    private Transform _center;
    private Vector3 _direction;
    private float _tempDist;
    private bool _turn;
    private float _neighborDistance;
    
    protected GameObject ThisEnemy;
    protected GameObject Player;
    protected float RotationSpeed;
    protected DolphinSpawner DolphinSpawner;
    protected float MoveSpeed;
    
    public Idle(GameObject thisEnemy, GameObject player, float neighborDistance, float moveSpeed, float rotationSpeed, DolphinSpawner dolphinSpawner)
    {
        _turn = false;
        ThisEnemy = thisEnemy;
        Player = player;
        _neighborDistance = neighborDistance;
        RotationSpeed = rotationSpeed;
        DolphinSpawner = dolphinSpawner;
        MoveSpeed = moveSpeed;
    }
    
    public override void Enter()
    {
        
    }

    public override void Update()
    {
        _center = DolphinSpawner.spawnPoint.transform;

        if (DolphinSpawner)
        {
            _tempDist = Vector3.Distance(ThisEnemy.transform.position, _center.position);

            float dist = Vector3.Distance(ThisEnemy.transform.position, _center.position);
            
            if ( dist >= DolphinSpawner.effectiveRadius)
            {
                _turn = true;
            }
            else
            {
                _turn = false;
            }

            if (_turn)
            {
                _direction = (_center.position - ThisEnemy.transform.position).normalized;

                Quaternion lookRotation = Quaternion.LookRotation(_direction);
                
                ThisEnemy.transform.rotation = Quaternion.Slerp(ThisEnemy.transform.rotation, lookRotation,
                    RotationSpeed * Time.deltaTime);
            }
            else
            {
                if (Random.Range(0, 5) < 3)
                {
                    ApplyRules();
                }
            }
        }
        
        ThisEnemy.transform.Translate(Time.deltaTime * MoveSpeed * Vector3.forward);
        
        Vector3 position = ThisEnemy.transform.position;
        
        if (position.y > GameManager.Instance.WaterLevel)
        {
            position.y = GameManager.Instance.WaterLevel;
        }
        ThisEnemy.transform.position = position;

        ThisEnemy.transform.rotation = Quaternion.Euler(0, ThisEnemy.transform.rotation.eulerAngles.y, 0);
    }

    public override void Exit()
    {
        
    }
    
    void ApplyRules()
    {
        GameObject[] pod = DolphinSpawner.allDolphins;

        Vector3 vcenter =_center.position;
        Vector3 vavoid = _center.position;
        Vector3 goalPosition = DolphinSpawner.goalPosition;
        float gSpeed = 0.1f;
        float distance;
        int groupSize = 0;

        foreach (GameObject dolphin in pod)
        {
            if (dolphin != ThisEnemy)
            {
                distance = Vector3.Distance(dolphin.transform.position, ThisEnemy.transform.position);

                if (distance <= _neighborDistance)
                {
                    vcenter += dolphin.transform.position;
                    groupSize++;

                    if (distance < 1.0f)
                    {
                        vavoid += (ThisEnemy.transform.position - dolphin.transform.position);
                    }

                    Dolphin anotherDolphin = dolphin.GetComponent<Dolphin>();
                    gSpeed += anotherDolphin.MoveSpeed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPosition - ThisEnemy.transform.position);

            Vector3 direction = (vcenter + vavoid) - ThisEnemy.transform.position;

            if (direction != Vector3.zero)
            {
                ThisEnemy.transform.rotation = Quaternion.Slerp(ThisEnemy.transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);
            }
        }
    }
}