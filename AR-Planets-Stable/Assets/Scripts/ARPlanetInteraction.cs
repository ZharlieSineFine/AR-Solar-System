using UnityEngine;

public class ARPlanetInteraction : MonoBehaviour
{
    private Vector2 lastTouchPosition1;
    private Vector2 lastTouchPosition2;
    private float initialDistance;
    private float currentDistance;

    void Update()
    {
        // Handle pinch zoom
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                lastTouchPosition1 = touch1.position;
                lastTouchPosition2 = touch2.position;
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
            }

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float zoomFactor = currentDistance / initialDistance;
                transform.localScale *= zoomFactor;
                initialDistance = currentDistance;
            }
        }

        // Handle rotation
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float rotationX = touch.deltaPosition.y * 0.1f;
                float rotationY = touch.deltaPosition.x * 0.1f;
                transform.Rotate(Vector3.right, -rotationX, Space.World);
                transform.Rotate(Vector3.up, rotationY, Space.World);
            }
        }
    }
}