using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterMesh : MonoBehaviour
{
    public int width = 50;       // Higher resolution for smoother waves
    public int height = 50;
    public float size = 1f;      // Distance between vertices
    public float waveSpeed = 1f; // Speed of the wave animation
    public float waveHeight = 2f; // Height of the waves
    public float waveFrequency = 1f; // Frequency of the waves

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] originalVertices;

    void Start()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(width + 1) * (height + 1)];
        originalVertices = new Vector3[vertices.Length];
        int[] triangles = new int[width * height * 6];

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                int index = y * (width + 1) + x;
                vertices[index] = new Vector3(x * size, 0, y * size);
                originalVertices[index] = vertices[index];
            }
        }

        int triIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int topLeft = y * (width + 1) + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + (width + 1);
                int bottomRight = bottomLeft + 1;

                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = topRight;
                triangles[triIndex++] = topRight;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = bottomRight;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void Update()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 original = originalVertices[i];
            float wave = Mathf.Sin(Time.time * waveSpeed + original.x * waveFrequency) *
                         Mathf.Sin(Time.time * waveSpeed + original.z * waveFrequency);
            vertices[i] = new Vector3(original.x, wave * waveHeight, original.z);
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // Recalculate normals for proper shading
    }
}
