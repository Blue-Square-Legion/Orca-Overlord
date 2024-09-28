using System.Collections;
using System.Collections.Generic;

using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Swing : MonoBehaviour
{
    float pullStrength = 6000f;
    public float maxDistance = 40;
    public LayerMask swingableLayer;
    public GameObject predictionPoint;
    public InputActionProperty swingAction;
    public InputActionProperty pullAction;
    public Rigidbody playerRigidbody;
    public LineRenderer lineRenderer;
    private SpringJoint joint;
    private Vector3 swingPoint;
    private bool hasHit;
    public Transform startSwingHand;
    float _movementForce = 4f;
    float moveSpeed = 9f;
    Vector3 movement;
    bool _pull = false;
    public int rotationSpeed = 1;
    int speed = 1;
    public static bool _noPivot = false;
    float gunRange = 10000f;
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
    }

    void Update()
    {



        GetSwingPoint();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartSwing();
            _pull = true;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
            StopSwing();
        DrawRope();
        PullRope();
    }

    public void StartSwing()
    {
        if (hasHit)
        {
            joint = playerRigidbody.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;
            float distance = Vector3.Distance(playerRigidbody.position, swingPoint);
            joint.maxDistance = distance;
            joint.spring = 4.5f;
            joint.damper = 2f;
            joint.massScale = 4.5f;
        }
    }
    public void PullRope()
    {
        if (!joint)
            return;
        //    if (pullAction.action.IsPressed())
        if (_pull)
        {
            Vector3 direction = (swingPoint
                                 - startSwingHand.position).normalized;
            playerRigidbody.AddForce(direction * pullStrength * Time.deltaTime * 1.5f);
            float distance = Vector3.Distance(playerRigidbody.position, swingPoint);
            joint.maxDistance = distance;
        }
        _pull = false;
    }
    public void StopSwing()
    {
        Destroy(joint);
    }
    public void GetSwingPoint()
    {
        if (joint)
        {
            predictionPoint.gameObject.SetActive(false);
            return;
        }
        RaycastHit raycastHit;
        hasHit = Physics.Raycast(startSwingHand.position, startSwingHand.forward, out raycastHit, maxDistance, swingableLayer);
        if (hasHit)
        {
            swingPoint = raycastHit.point;
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.transform.position = swingPoint;
            _noPivot = true;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }
    }
    public void DrawRope()
    {
        if (!joint)
        {
            lineRenderer.enabled = false;
        }
       else
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startSwingHand.position);
            lineRenderer.SetPosition(1, swingPoint * gunRange);

        }
      
    }
}
