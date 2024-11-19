using System.Collections.Generic;
using UnityEngine;

public class SolarSystemOrbit : MonoBehaviour
{
    public Transform sun; // 太阳的 Transform
    public List<Transform> planets; // 所有行星的 Transform 列表
    public List<float> orbitSpeeds; // 每个行星的公转速度

    void Update()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            // 检查行星和公转速度列表的长度是否一致
            if (i < orbitSpeeds.Count)
            {
                // 让行星围绕太阳的 Y 轴旋转
                planets[i].RotateAround(sun.position, Vector3.up, orbitSpeeds[i] * Time.deltaTime);
            }
        }
    }
}
