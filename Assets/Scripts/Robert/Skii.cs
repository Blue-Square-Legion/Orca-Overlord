using UnityEngine;
using UnityEngine.InputSystem;


public class skii : MonoBehaviour
{
    float pullStrength = 6000f;
    public float maxDistance = 40;
    public LayerMask swingableLayer;
    public GameObject dolphin;
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
    private void Start()
    {


        RaycastHit raycastHit;
        hasHit = Physics.Raycast(startSwingHand.position, startSwingHand.forward, out raycastHit, maxDistance);
        if (hasHit)
        {
            swingPoint = raycastHit.point;
            dolphin.gameObject.SetActive(true);
            dolphin.transform.position = swingPoint;
            _noPivot = true;
        }
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
}