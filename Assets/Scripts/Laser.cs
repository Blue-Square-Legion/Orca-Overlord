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
    [SerializeField] public Swing swing;
    public Camera mainCamera;
    public LineRenderer laserline;
    //  public LineRenderer lineRenderer;
    public Transform hand;
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
            hand.transform.Rotate(-.2f, 0f, 0f);
        if (Input.GetKey(KeyCode.S))
            hand.transform.Rotate(.2f, 0f, 0f);
        if (Input.GetKey(KeyCode.D))
            hand.transform.Rotate(0f, .2f, 0f);
        if (Input.GetKey(KeyCode.A))
            hand.transform.Rotate(0f, -.2f, 0f);

       // RaycastHit hit;
        laserline.positionCount = 2;
        laserline.SetPosition(0, hand.transform.position);
        if (!Swing._noPivot)
        {

            laserline.SetPosition(1, hand.transform.forward * 10000);
        }
        else
        {
            laserline.SetPosition(1, swing.predictionPoint.transform.position);


        }
    }
}
