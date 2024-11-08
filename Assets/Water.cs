using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<FirstPersonMovement>() != null)
        {
            FirstPersonMovement movement = other.GetComponent<FirstPersonMovement>();
            movement.isSwimming = true;
            other.GetComponent<Jump>().enabled = false;
            other.GetComponent<Crouch>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<FirstPersonMovement>() != null)
        {
            FirstPersonMovement movement = other.GetComponent<FirstPersonMovement>();
            movement.isSwimming = false;
            other.GetComponent<Jump>().enabled = true;
            other.GetComponent<Crouch>().enabled = true;
        }
    }
}