using System.Collections;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public Transform[] floaters;
    public float underWaterDrag;
    public float underWaterAngularDrag;
    public float airDrag;
    public float airAngularDrag;
    public float floatingPower;
    public float depthThreshold;
    public float maxDepth;
    public bool randomness = false;
    public float depth;
    public Transform waterSurface;

    private bool underWater = false;
    private int floatersUnderWater;
    private float randomnessVal = 1f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (randomness)
        {
            StartCoroutine(SetRandomness());
        }
    }

    private void FixedUpdate()
    {
        floatersUnderWater = 0;

        foreach(Transform floater in floaters)
        {
            depth = waterSurface.position.y - floater.position.y;

            if (gameObject.TryGetComponent(out Boat boat))
            {
                if (boat.isFlipped)
                {
                    return;
                }
            }
            
            if (depth > depthThreshold && depth < maxDepth)
            {
                rb.AddForceAtPosition(Vector3.up * floatingPower * randomnessVal * depth, floater.position, ForceMode.Force);
                
                floatersUnderWater++;
                
                if (!underWater)
                {
                    underWater = true;
                    SwitchState(underWater);
                }
            }

        }

        if (underWater && floatersUnderWater == 0)
        {
            underWater = false;
            SwitchState(underWater);
        }
    }

    void SwitchState(bool isUnderWater)
    {
        if (isUnderWater)
        {
            rb.drag = underWaterDrag;
            rb.angularDrag = underWaterAngularDrag;
        }
        else
        {
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }

    private IEnumerator SetRandomness()
    {
        yield return new WaitForSeconds(.2f);
        StartCoroutine(SetRandomness());
        randomnessVal = Random.Range(.5f, 2f);
    }
}
