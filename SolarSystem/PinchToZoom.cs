using UnityEngine;
using UnityEngine.InputSystem;

public class PinchToZoom : MonoBehaviour
{
    public Transform solarSystemParent; 
    public float zoomSpeed = 0.01f;      
    public float minScale = 0.5f;        
    public float maxScale = 3f;         
    private Vector3 initialScale;        

    public LineRenderer[] orbitPaths;    
    private float[] initialRadii;        

    private int uranusIndex = 6; 

    void Start()
    {
        if (solarSystemParent == null)
        {
            Debug.LogError("Please set SolarSystemParent as the reference object of the script!");
            return;
        }

        initialScale = solarSystemParent.localScale;

     
        orbitPaths = solarSystemParent.GetComponentsInChildren<LineRenderer>();

    
        initialRadii = new float[orbitPaths.Length];
        for (int i = 0; i < orbitPaths.Length; i++)
        {
            if (orbitPaths[i] != null)
            {
                orbitPaths[i].enabled = true;

                if (orbitPaths[i].positionCount == 0)
                {
                    orbitPaths[i].positionCount = 100;  
                }

                if (i == uranusIndex)
                {
                  
                    float radius = 5.0f; 
                    Vector3[] points = new Vector3[100]; 
                    for (int j = 0; j < points.Length; j++)
                    {
                        float angle = j * Mathf.PI * 2 / points.Length;
                        points[j] = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle)); 
                    }
                    orbitPaths[i].SetPositions(points); 
                }

                initialRadii[i] = orbitPaths[i].GetPosition(0).magnitude;
                Debug.Log($"Orbit {i + 1}: Initial Radius = {initialRadii[i]}");
            }
        }

        Debug.Log("Total Orbits: " + orbitPaths.Length);
    }

    void Update()
    {
        
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            Vector2 touch1Pos = Touchscreen.current.touches[0].position.ReadValue();
            Vector2 touch2Pos = Touchscreen.current.touches[1].position.ReadValue();

            float currentDistance = Vector2.Distance(touch1Pos, touch2Pos);
            float previousDistance = Vector2.Distance(
                Touchscreen.current.touches[0].position.ReadValue() - Touchscreen.current.touches[0].delta.ReadValue(),
                Touchscreen.current.touches[1].position.ReadValue() - Touchscreen.current.touches[1].delta.ReadValue()
            );

            float distanceDelta = currentDistance - previousDistance;
            Zoom(distanceDelta * zoomSpeed);  
        }

   
        for (int i = 0; i < orbitPaths.Length; i++)
        {
            if (orbitPaths[i] != null)
            {
         
                UpdateOrbitPathRadius(orbitPaths[i], initialRadii[i] * solarSystemParent.localScale.x);
            }
        }
    }

    private void Zoom(float increment)
    {

        Vector3 newScale = solarSystemParent.localScale + Vector3.one * increment;
        newScale = Vector3.Max(newScale, initialScale * minScale);
        newScale = Vector3.Min(newScale, initialScale * maxScale);
        solarSystemParent.localScale = newScale;
    }

    private void UpdateOrbitPathRadius(LineRenderer lineRenderer, float newRadius)
    {
        int pointsCount = lineRenderer.positionCount;

 
        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 point = lineRenderer.GetPosition(i);
            point = point.normalized * newRadius;
            lineRenderer.SetPosition(i, point);
        }

        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, lineRenderer.transform.position);
        float width = Mathf.Clamp(0.1f / distanceFromCamera, 0.001f, 0.005f);  
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
}
