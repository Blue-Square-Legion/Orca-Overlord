using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private SpringJoint joint;

    private LineRenderer lineRenderer;

    public LayerMask grappableLayer;

    public float maxDistance;
    public float maxJointLength;
    public float minJointLength;
    public float jointSpring;
    public float jointDamper;
    public float jointScale;


    public Transform cam;
    public Transform waist;
    public Transform player;
   
    private Transform ancorPoint;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        playerInput();

        if(joint)
        joint.connectedAnchor = ancorPoint.position;
    }

    private void playerInput()
    {
        //grappling
        if (Input.GetMouseButtonDown(1))
        {
            StartGraple();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            StopGraple();
        }
        //
    }

    private void LateUpdate()
    {
        drawLine();
        if (Input.GetMouseButtonDown(0)) { 
            lineRenderer.SetPosition(0, waist.position);
        lineRenderer.SetPosition(1, waist.position);
            }
    }

    private void StopGraple()
    {
        //ancorPoint.gameObject.GetComponent<Dolphin>().isConnected = false;
        GameObject.Destroy(joint);
        lineRenderer.positionCount = 0;
    }

    private void StartGraple()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, grappableLayer))
        {
            ancorPoint = hit.collider.gameObject.transform;
            //ancorPoint.gameObject.GetComponent<Dolphin>().isConnected = true;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = ancorPoint.position;

            float distanceFromPoint = Vector3.Distance(player.position, ancorPoint.position);

            joint.maxDistance = distanceFromPoint * maxJointLength;
            joint.minDistance = distanceFromPoint * minJointLength;

            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointScale;

            lineRenderer.positionCount = 2;

        }
    }

    private void drawLine()
    {
        if (!joint) return;

        lineRenderer.SetPosition(0, waist.position);
        lineRenderer.SetPosition(1, ancorPoint.position);
    }
}
