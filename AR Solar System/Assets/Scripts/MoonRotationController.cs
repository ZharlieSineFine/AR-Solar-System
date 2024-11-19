using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotationController : MonoBehaviour
{
    public GameObject MoonObject;
    public Vector3 RotationVector = new Vector3(0, 0.5f, 0);

    private const float MoonAxialTilt = 1.5f;

    private void Start()
    {
        MoonObject.transform.rotation = Quaternion.Euler(MoonAxialTilt, 0, 0);
    }

    private void Update()
    {
        MoonObject.transform.Rotate(RotationVector * Time.deltaTime);
    }
}