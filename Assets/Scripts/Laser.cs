using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
public class Laser : MonoBehaviour
{

    public GameObject hut;
    public GameObject Sphere;
   // [SerializeField] public Swing swing;
    public Camera mainCamera;
    public LineRenderer laserline;
    //  public LineRenderer lineRenderer;
    public Transform player;
    //   public GameObject Sphere1, Sphere2, Sphere3, Sphere4, Sphere5, Sphere6;
    float animDuration = 2f;
    float gunRange = 100f;
    // Start is called before the first frame update
    void Start()
    {
        laserline = GetComponent<LineRenderer>();
        // StartCoroutine(DrawLine());
    }






            void Update()
    {
        if (Input.GetKey(KeyCode.E))
            player.transform.Rotate(0f, .3f, 0f);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
           player.transform.LookAt(hitInfo.point);


        UnityEngine.Vector3 fwd = player.transform.TransformDirection(UnityEngine.Vector3.forward);

        if (Physics.Raycast(player.transform.position, fwd, 1000) && Input.GetKey(KeyCode.Mouse0))
        {

            Sphere.transform.position = UnityEngine.Vector3.MoveTowards(Sphere.transform.position, player.transform.position, 110f * Time.deltaTime);
        }
        if (Physics.Raycast(player.transform.position, fwd, 1000) && Input.GetKey(KeyCode.Mouse1))
        {

            player.transform.position = UnityEngine.Vector3.MoveTowards(player.transform.position, hut.transform.position, 150f * Time.deltaTime);

        }





        laserline.positionCount = 2;
            laserline.SetPosition(0, player.transform.position);
            if (!Swing._noPivot)
            {

                laserline.SetPosition(1, player.transform.forward * 10000);
            }
   //    else
       //   {
           //   laserline.SetPosition(1, swing.predictionPoint.transform.position);


   //   }
        }
    }
