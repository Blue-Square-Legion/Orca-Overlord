using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public Transform[] floaters;
    public float underWaterDrag;
    public float underWaterAngularDrag;
    public float airDrag;
    public float airAngularDrag;
    public float floatingPower;

    public Transform waterSurface;

    private bool underWater = false;
    private int floatersUnderWater;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        floatersUnderWater = 0;

        foreach(Transform floater in floaters)
        {
            float difference = floater.position.y - waterSurface.position.y;

            if (difference < 0)
            {
                rb.AddForceAtPosition(Vector3.up * floatingPower, floater.position);
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
}
