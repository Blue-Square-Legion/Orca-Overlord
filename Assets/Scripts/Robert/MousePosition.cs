using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics.Contracts;

public class MousePosition : MonoBehaviour
{
   // public Vector3 screenPosition;
 //   public Vector3 worldPosition;
   // public LayerMask layerMask;
    //[SerializeField] Transform target;
    public GameObject Player;
    float moveSpeed = 2f;
    public float speed = 2f;

  public  Vector3 movementDirection;

    private void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()

    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.RightArrow))
        {
           Player.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
           Player.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
           Player.transform.Translate(Vector3.forward * -moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        { 
            Player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
       

        if (Input.GetKey(KeyCode.M))
            {
                Player.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            }
        if (Input.GetKey(KeyCode.N))
        {
            Player.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
            Player.transform.Rotate(0f, .3f, 0f);


    }

}

