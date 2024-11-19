using UnityEngine;

public class PinchToZoom : MonoBehaviour
{
    public Transform solarSystemParent;  // 太阳系父物体
    public float zoomSpeed = 0.001f;      // 缩放速度
    public float minScale = 0.5f;        // 最小缩放值
    public float maxScale = 3f;          // 最大缩放值
    private Vector3 initialScale;        // 初始缩放值

    public LineRenderer[] orbitPaths;    // 轨道路径
    private float[] initialRadii;        // 初始轨道半径

    void Start()
    {
        // 确保solarSystemParent已经设置
        if (solarSystemParent == null)
        {
            Debug.LogError("Please set SolarSystemParent as the reference object of the script!");
            return;
        }

        initialScale = solarSystemParent.localScale;

        // 获取所有轨道的LineRenderer组件
        orbitPaths = solarSystemParent.GetComponentsInChildren<LineRenderer>();

        // 存储轨道的初始半径
        initialRadii = new float[orbitPaths.Length];
        for (int i = 0; i < orbitPaths.Length; i++)
        {
            // 这里假设轨道路径是圆形的，获取初始半径（轨道的第一个点到中心的距离）
            initialRadii[i] = Vector3.Distance(orbitPaths[i].GetPosition(0), solarSystemParent.position);
        }
    }

    void Update()
    {
        // 检查是否有两根触摸手指
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // 获取当前距离和上一帧的距离
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            float previousDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);

            // 计算距离差
            float distanceDelta = currentDistance - previousDistance;

            // 调用缩放方法
            Zoom(distanceDelta * zoomSpeed);
        }
    }

    // 缩放太阳系和轨道
    private void Zoom(float increment)
    {
        // 更新父物体的缩放
        Vector3 newScale = solarSystemParent.localScale + Vector3.one * increment;

        // 限制缩放范围
        newScale = Vector3.Max(newScale, initialScale * minScale);
        newScale = Vector3.Min(newScale, initialScale * maxScale);

        // 更新父物体的缩放
        solarSystemParent.localScale = newScale;

        // 更新轨道路径的半径
        for (int i = 0; i < orbitPaths.Length; i++)
        {
            // 根据新的缩放比例更新轨道的半径
            float newRadius = initialRadii[i] * newScale.x / initialScale.x;

            // 重新生成轨道路径
            UpdateOrbitPath(orbitPaths[i], newRadius);
        }
    }

    // 重新生成轨道路径
    private void UpdateOrbitPath(LineRenderer lineRenderer, float newRadius)
    {
        // 获取当前轨道的点数量
        int pointsCount = lineRenderer.positionCount;

        // 检查LineRenderer是否足够细，避免轨道看不见
        if (lineRenderer.startWidth == 0)
        {
            lineRenderer.startWidth = 0.01f;  // 确保轨道有一个合理的宽度
            lineRenderer.endWidth = 0.01f;
        }

        // 重新计算轨道路径的每个点位置
        for (int i = 0; i < pointsCount; i++)
        {
            float angle = i * Mathf.PI * 2 / pointsCount;  // 均匀分布在圆周上的角度
            float x = Mathf.Cos(angle) * newRadius;       // 计算X坐标
            float z = Mathf.Sin(angle) * newRadius;       // 计算Z坐标
            Vector3 newPosition = new Vector3(x, 0, z);   // Y坐标不变，假设轨道在XY平面

            // 更新轨道路径
            lineRenderer.SetPosition(i, newPosition);
        }
    }
}
