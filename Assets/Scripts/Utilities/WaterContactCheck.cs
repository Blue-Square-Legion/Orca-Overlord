using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterContactCheck : MonoBehaviour
{
    private Boat _parentBoat;

    private void Awake()
    {
        _parentBoat = GetComponentInParent<Boat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _parentBoat.isInWater = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    { 
        if (other.CompareTag("Water"))
        {
            _parentBoat.isInWater = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
