using UnityEngine;


public class Floater : MonoBehaviour
{
   // [SerializeField] public WaveManager waveManager;
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floatCount = 9;
    public float waterDrag = .99f;
    public float waterAngularDrag = .5f;
   

    void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floatCount, transform.position, ForceMode.Acceleration);
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        
       // rigidBody.transform.position = new UnityEngine.Vector3(transform.position.x,(transform.position.y)-Mathf.Sin(1*Time.deltaTime),transform.position.z);
        if (transform.position.y <= waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime);
        }
    }
}