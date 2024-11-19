using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusRotationController : MonoBehaviour
{
    public GameObject PlanetObject; // The Venus object
    public Vector3 RotationVector = new Vector3(0, -1.0f, 0); // Rotation speed of Venus
    public Transform Sun; // Reference to the Sun
    public float orbitalSpeed = 35.02f; // Venus's average orbital speed (scaled)
    public float orbitalRadius = 0.72f; // Average distance from the Sun in AU (scaled)
    public int lineSegments = 100; // Number of segments in the orbital path
    private LineRenderer lineRenderer; // LineRenderer to draw the orbital path

    private const float VenusAxialTilt = 177.4f; // Venus's axial tilt in degrees
    private float angle; // Current angle of Venus in its orbit

    private void Start()
    {
        // Set Venus's initial tilt
        PlanetObject.transform.rotation = Quaternion.Euler(VenusAxialTilt, 0, 0);

        // Initialize the LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = lineSegments + 1;
        lineRenderer.startWidth = 0.05f; // Adjust width for visibility
        lineRenderer.endWidth = 0.05f;
        lineRenderer.loop = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white; // Color for Venus's orbit
        lineRenderer.endColor = Color.white;

        DrawOrbit();
    }

    private void Update()
    {
        // Rotate Venus on its axis
        PlanetObject.transform.Rotate(RotationVector * Time.deltaTime);

        // Update the angle based on the orbital speed
        angle += orbitalSpeed * Time.deltaTime * 0.01f; // Adjust speed scaling as needed

        // Calculate the new position of Venus in its orbit
        float x = Mathf.Cos(angle) * orbitalRadius;
        float z = Mathf.Sin(angle) * orbitalRadius;

        // Set the new position of Venus
        PlanetObject.transform.position = new Vector3(x, 0, z);
    }

    private void DrawOrbit()
    {
        for (int i = 0; i <= lineSegments; i++)
        {
            float theta = 2.0f * Mathf.PI * i / lineSegments; // Angle in radians
            float x = Mathf.Cos(theta) * orbitalRadius;
            float z = Mathf.Sin(theta) * orbitalRadius;
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
    }
}