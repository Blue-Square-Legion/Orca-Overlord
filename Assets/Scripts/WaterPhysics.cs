using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterPhysics : MonoBehaviour
{
    public float waveSpeed = 0.5f;  // Speed of wave movement
    public float waveScale = 1f;    // Scale of the waves (affects wave frequency)
    public float waveHeight = 1f;   // Height of the waves
    public float waveTimeOffset = 0f;  // Starting offset to make the waves move at different times
    private Mesh waterMesh;          // Reference to the mesh
    private Vector3[] vertices;      // Store vertices of the mesh
    private Vector3[] originalVertices; // Store original vertices positions for stability

    private float timeOffset;        // Used to animate the waves over time

    void Start()
    {
        // Get the MeshFilter and mesh from the water object
        waterMesh = GetComponent<MeshFilter>().mesh;
        vertices = waterMesh.vertices;

        // Store the original vertices positions to prevent moving the mesh
        originalVertices = new Vector3[vertices.Length];
        vertices.CopyTo(originalVertices, 0);

        // Time offset for animated wave movement
        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Animate the water mesh using Perlin noise
        UpdateWaveMesh();
    }

    void UpdateWaveMesh()
    {
        // Loop through each vertex and apply the Perlin noise function
        for (int i = 0; i < vertices.Length; i++)
        {
            // Get the local position of the vertex
            Vector3 vertex = originalVertices[i];

            // Apply Perlin noise for wave height (y)
            float waveY = Mathf.PerlinNoise((vertex.x + timeOffset) * waveScale, (vertex.z + timeOffset) * waveScale);
            waveY = waveY * waveHeight;  // Scale the Perlin noise to control wave height

            // Modify the vertex position (only change the y value)
            vertices[i].y = waveY;
        }

        // Update the mesh with the new vertex positions
        waterMesh.vertices = vertices;
        waterMesh.RecalculateNormals();  // Recalculate normals for correct lighting/shading

        // To create moving waves, update the timeOffset continuously
        timeOffset += waveSpeed * Time.deltaTime;  // Change this to move the waves over time
    }

    // This function will be called by the WaterInteraction script
    public float GetWaveHeightAtPosition(float x, float z)
    {
        // Convert world positions (x, z) to local positions in the water mesh
        Vector3 localPosition = transform.InverseTransformPoint(new Vector3(x, 0, z));

        // Calculate Perlin noise for this position
        float waveY = Mathf.PerlinNoise((localPosition.x + timeOffset) * waveScale, (localPosition.z + timeOffset) * waveScale);
        return waveY * waveHeight;  // Return wave height
    }
}