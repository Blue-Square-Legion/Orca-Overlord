using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class MoveFace : MonoBehaviour
{
    public float speed =8f;

    Vector3 movementDirection;




    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
       // transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

      //  if (movementDirection != Vector3.zero)
     //   {
           // UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(movementDirection,Vector3.up);
        //    transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, speed*Time.deltaTime);
     //   }

        transform.forward =-movementDirection;//(in above if)
    }
}