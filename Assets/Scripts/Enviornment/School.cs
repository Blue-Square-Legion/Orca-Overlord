using UnityEditor.ShaderKeywordFilter;
using UnityEngine;


/// <summary>
/// Handles movement of fish spawned by GlobalFlock script.
/// </summary>
public class School : MonoBehaviour
{
    public float speed = 2.0f;
    private float rotationSpeed = 4.0f;
    private Vector3 averageHeading;
    private Vector3 averagePosition;
    private float neighborDistance = 3.0f;
    private bool turn = false;
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        speed = Random.Range(2.0f, 5.0f);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= FishSchoolManager.fishSpawnOffset)
        {
            turn = true;
        }
        else
        {
            turn = false;
        }

        if (turn)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }    
        }
        
        transform.Translate(Time.deltaTime * speed, 0, 0);    
    }

    void ApplyRules()
    {
        GameObject[] school;
        school = FishSchoolManager.allFish;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPosition = FishSchoolManager.GoalPosition;

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

                    School anotherSchool = fish.GetComponent<School>();
                    gSpeed += anotherSchool.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPosition - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
