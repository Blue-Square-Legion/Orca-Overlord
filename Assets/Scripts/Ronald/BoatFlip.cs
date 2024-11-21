using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFlip : MonoBehaviour
{
    //I could not figure out how to make this work, moving to animation instead

    public float rotateSpeed = 10f;
    public bool isTouched = false;
    private float timeElapsed = 0f;
    public float rotationDuration = 1f;
    public bool flipped = false;

    // Update is called once per frame
    void Update()
    {
        if (isTouched)
        {
            FlipBoat();
            isTouched = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouched = true;
        }
    }

    public void FlipBoat()
    {
        //flip the boat on the z axis in 1 second
        Debug.Log("Player flipped boat");
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / rotationDuration;
        Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), t);

        if (t >= 1f)
        {
            isTouched = false;  // Stop the rotation once it's complete
            flipped = true; 
        }

        //stop the boat from spinning in every direction
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
