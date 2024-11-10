using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PinchToZoom : MonoBehaviour
{
    public Transform solarSystemParent;  // 父物体，包含太阳系
    public float zoomSpeed = 0.01f;      // 缩放速度
    public float minScale = 0.5f;        // 最小缩放比例
    public float maxScale = 3f;          // 最大缩放比例
    private Vector3 initialScale;         // 初始缩放值

    public LineRenderer[] orbitPaths;    // 存储所有轨道的 LineRenderer
    private float[] initialRadii;        // 存储每个轨道的初始半径
    private Transform[] planets;         // 存储所有行星的 Transform

    void Start()
{
    if (solarSystemParent == null)
    {
        Debug.LogError("Please set SolarSystemParent as the reference object of the script!");
        return;
    }

    initialScale = solarSystemParent.localScale;

    // 获取所有轨道的 LineRenderer
    List<LineRenderer> orbitPathsList = new List<LineRenderer>();
    GetAllLineRenderers(solarSystemParent, orbitPathsList);
    orbitPaths = orbitPathsList.ToArray();

    // 检查轨道数量
    Debug.Log("Total Orbits Found: " + orbitPaths.Length);
    for (int i = 0; i < orbitPaths.Length; i++)
    {
        Debug.Log("Orbit " + i + ": " + orbitPaths[i].gameObject.name);
    }

    // 存储每个轨道的初始半径
    initialRadii = new float[orbitPaths.Length];
    for (int i = 0; i < orbitPaths.Length; i++)
    {
        initialRadii[i] = orbitPaths[i].GetPosition(0).magnitude;
    }

    // 启用所有轨道的 LineRenderer
    foreach (var lineRenderer in orbitPaths)
    {
        if (!lineRenderer.enabled)
        {
            Debug.LogWarning("LineRenderer for orbit is disabled.");
            lineRenderer.enabled = true;
        }
    }
}
    void Update()
    {
        // 确认触摸输入并进行缩放操作
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            Vector2 touch1Pos = Touchscreen.current.touches[0].position.ReadValue();
            Vector2 touch2Pos = Touchscreen.current.touches[1].position.ReadValue();

            // 获取当前和前一帧的触摸距离
            float currentDistance = Vector2.Distance(touch1Pos, touch2Pos);
            float previousDistance = Vector2.Distance(
                Touchscreen.current.touches[0].position.ReadValue() - Touchscreen.current.touches[0].delta.ReadValue(),
                Touchscreen.current.touches[1].position.ReadValue() - Touchscreen.current.touches[1].delta.ReadValue()
            );

            // 根据触摸的距离变化进行缩放
            float distanceDelta = currentDistance - previousDistance;
            Zoom(distanceDelta * zoomSpeed);  // 执行缩放
        }

        // 确保轨道实时更新
        for (int i = 0; i < orbitPaths.Length; i++)
        {
            UpdateOrbitPathRadius(orbitPaths[i], initialRadii[i] * solarSystemParent.localScale.x);
        }

        // 调整行星位置，根据父物体的缩放进行缩放
        foreach (var planet in planets)
        {
            if (planet != null)
            {
                planet.localScale = solarSystemParent.localScale; // 行星缩放
            }
        }
    }

    // 递归获取所有子对象中的 LineRenderer
    private void GetAllLineRenderers(Transform parent, List<LineRenderer> lineRenderers)
    {
        foreach (Transform child in parent)
        {
            LineRenderer lr = child.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lineRenderers.Add(lr);
            }
            GetAllLineRenderers(child, lineRenderers); // 递归查找子对象
        }
    }

    // 缩放太阳系和轨道
    private void Zoom(float increment)
    {
        // 计算新的缩放比例
        Vector3 newScale = solarSystemParent.localScale + Vector3.one * increment;

        // 限制缩放比例
        newScale = Vector3.Max(newScale, initialScale * minScale);
        newScale = Vector3.Min(newScale, initialScale * maxScale);

        // 更新太阳系的缩放
        solarSystemParent.localScale = newScale;

        // 更新轨道的半径
        for (int i = 0; i < orbitPaths.Length; i++)
        {
            float newRadius = initialRadii[i] * newScale.x / initialScale.x;
            UpdateOrbitPathRadius(orbitPaths[i], newRadius);
        }
    }

    // 更新轨道的半径和宽度
    private void UpdateOrbitPathRadius(LineRenderer lineRenderer, float newRadius)
    {
        int pointsCount = lineRenderer.positionCount;

        // 更新轨道的每个点
        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 point = lineRenderer.GetPosition(i);
            point = point.normalized * newRadius; // 按新的半径更新轨道
            lineRenderer.SetPosition(i, point);
        }

        // 根据距离和缩放调整轨道宽度
        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, lineRenderer.transform.position);
        float width = Mathf.Clamp(0.1f / distanceFromCamera, 0.01f, 0.05f);  // 根据距离动态调整宽度，最小宽度 0.01，最大宽度 0.05

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
}
