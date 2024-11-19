using UnityEngine;

public class OrbitPath : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float radiusX = 5.0f; 
    public float radiusY = 5.0f; 
    public int segments = 100;   

    void Start()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = segments + 1;
        CreateOrbitPath();
    }

    void CreateOrbitPath()
    {
        Vector3[] positions = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle) * radiusX;
            float z = Mathf.Sin(angle) * radiusY;
            positions[i] = new Vector3(x, 0, z);
        }
        lineRenderer.SetPositions(positions);
    }
}
