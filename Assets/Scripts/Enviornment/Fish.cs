using System;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Handles movement of fish spawned by GlobalFlock script.
/// </summary>
public class Fish : MonoBehaviour
{
    [SerializeField] private float minSpeed = 1.0f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 4.0f;
    [SerializeField] private float neighborDistance = 3.0f;
    
    private FishManager _fishManager;
    private float _speed;
    private GameObject _water;
    private Vector3 _center;
    private Vector3 _averageHeading;
    private Vector3 _averagePosition;
    
    private bool _turn = false;

    private void Awake()
    {
        _fishManager = GameManager.FishManager;
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _center = _fishManager.spawnPoint.transform.position;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (_fishManager)
        {
            if (Vector3.Distance(transform.position, _center) >= _fishManager.effectiveRadius)
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
                if (Random.Range(0, 5) < 1)
                {
                    ApplyRules();
                }    
            }
        
            transform.Translate(Time.deltaTime * _speed, 0, 0);    
        }
    }

    void ApplyRules()
    {
        GameObject[] school;
        school = _fishManager.allFish;

        Vector3 vcenter =_center;
        Vector3 vavoid = _center;
        float gSpeed = 0.1f;

        Vector3 goalPosition = _fishManager.GoalPosition;

        float distance;

        int groupSize = 0;

        foreach (GameObject fish in school)
        {
            if (fish != this.gameObject)
            {
                distance = Vector3.Distance(fish.transform.position, this.transform.position);

                if (distance <= neighborDistance)
                {
                    vcenter += fish.transform.position;
                    groupSize++;

                    if (distance < 1.0f)
                    {
                        vavoid += (this.transform.position - fish.transform.position);
                    }

                    Fish anotherFish = fish.GetComponent<Fish>();
                    gSpeed += anotherFish._speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPosition - this.transform.position);
            _speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
