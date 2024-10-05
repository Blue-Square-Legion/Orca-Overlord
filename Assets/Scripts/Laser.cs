using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class Laser : MonoBehaviour
{
    UnityEngine.Vector3 mousePos;
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
        if (Input.GetKey(KeyCode.W))
            player.transform.Rotate(-.2f, 0f, 0f);
        if (Input.GetKey(KeyCode.S))
            player.transform.Rotate(.2f, 0f, 0f);
        if (Input.GetKey(KeyCode.D))
            player.transform.Rotate(0f, .2f, 0f);
        if (Input.GetKey(KeyCode.A))
            player.transform.Rotate(0f, -.2f, 0f);
        RaycastHit hit;
      //  GameObject[] gameObject = GameObject.FindGameObjectsWithTag("food");
        if (Physics.Raycast(player.transform.position, player.transform.TransformDirection(UnityEngine.Vector3.forward), out hit, Mathf.Infinity)&&UnityEngine.Input.GetKey(KeyCode.Mouse0))
          Sphere.transform.position  = UnityEngine.Vector3.MoveTowards(Sphere.transform.position, player.transform.position, 50f * Time.deltaTime);
        UnityEngine.Vector3 mousePos = Input.mousePosition;
       
        Ray ray =mainCamera.ScreenPointToRay(mousePos);
        
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                player.transform.LookAt(hit.point);
       

        laserline.positionCount = 2;
            laserline.SetPosition(0, player.transform.position);
            if (!Swing._noPivot)
            {

                laserline.SetPosition(1, player.transform.forward * 10000);
            }
         //   else
          //  {
             //   laserline.SetPosition(1, swing.predictionPoint.transform.position);


           // }
        }
    }
