using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float waterDensity = 1000f;  // Density of water
    public float gravity = 9.81f; // Gravity constant
    public float dragFactor = 0.1f; // Drag factor (resistance in water)
    public float restoreStrength = 10f; // Strength of the restoring force when the boat tilts

    private Rigidbody _rigidbody;
    private Vector3[] _originalVertices;  // Store original vertices positions for stability

    // Array of Transforms representing the floaters on the object
    public Transform[] floaters;

    private WaterPhysics _waterPhysics; // Reference to PerlinWaves script for wave height calculation

    private float _waterLevel;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _waterPhysics = FindObjectOfType<WaterPhysics>();
        _waterLevel = GameManager.Instance.WaterLevel;
        // Ensure floaters are set properly in the inspector
    }

    void FixedUpdate()
    {
        ApplyForces();
    }

    void ApplyForces()
    {
        // Apply buoyancy and forces based on Perlin noise wave heights
        float totalBuoyancyForce = 0f;

        // Calculate the average submersion depth and buoyancy force
        foreach (Transform floater in floaters)
        {
            Vector3 floaterPosition = floater.position;
            float waveHeightAtFloater = _waterPhysics.GetWaveHeightAtPosition(floaterPosition.x, floaterPosition.z);
            float submersionDepth = waveHeightAtFloater - floaterPosition.y - _waterLevel;

            if (submersionDepth > 0)
            {
                // Calculate buoyancy force: proportional to the submerged depth
                totalBuoyancyForce += waterDensity * submersionDepth * gravity;
            }

            // Optionally, apply drag based on the velocity of the boat
            Vector3 dragForce = -_rigidbody.velocity * dragFactor;
            _rigidbody.AddForce(dragForce, ForceMode.Force);
        }

        // Apply total buoyancy force to the rigidbody
        _rigidbody.AddForce(Vector3.up * totalBuoyancyForce, ForceMode.Force);

        // Apply restoring torque to simulate tilting
        ApplyRestoringTorque();
    }

    void ApplyRestoringTorque()
    {
        /*Floater Organization
         0 1
         2 3
         */
        
        // Calculate height differences between floaters to simulate tilting
        if (floaters.Length <= 0)
        {
            return;
        }
        
        Vector3 floater0Pos = floaters[0].position;
        Vector3 floater1Pos = floaters[1].position;
        Vector3 floater2Pos = floaters[2].position;
        Vector3 floater3Pos = floaters[3].position;

        // Calculate height differences between the front and back floaters (pitch)
        float frontHeight = (GetWaveHeightAtPosition(floater0Pos.x, floater0Pos.z) + GetWaveHeightAtPosition(floater1Pos.x, floater1Pos.z)) / 2;
        
        float backHeight = (GetWaveHeightAtPosition(floater2Pos.x, floater2Pos.z) + GetWaveHeightAtPosition(floater3Pos.x, floater3Pos.z)) / 2;
        
        float pitchDifference = frontHeight - backHeight;

        
        // Calculate height differences between the left and right floaters (roll)
        float leftHeight = (GetWaveHeightAtPosition(floater0Pos.x, floater0Pos.z) + GetWaveHeightAtPosition(floater2Pos.x, floater2Pos.z)) / 2;
        
        float rightHeight = (GetWaveHeightAtPosition(floater1Pos.x, floater1Pos.z) + GetWaveHeightAtPosition(floater3Pos.x, floater3Pos.z)) / 2;
        
        float rollDifference = leftHeight - rightHeight;

        
        // Apply torques for pitch and roll based on height differences
        Vector3 torque = Vector3.zero;
        torque.x = pitchDifference * restoreStrength;  // Apply pitch torque
        torque.z = -rollDifference * restoreStrength; // Apply roll torque

        // Apply the calculated torque to the boat
        _rigidbody.AddTorque(torque, ForceMode.Force);
    }

    float GetWaveHeightAtPosition(float x, float z)
    {
        return _waterPhysics.GetWaveHeightAtPosition(x, z);
    }
}