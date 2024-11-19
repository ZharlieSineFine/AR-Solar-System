using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotationController : MonoBehaviour
{
    public GameObject SunObject;
    public Vector3 RotationVector = new Vector3(0, 1.0f, 0);

    private const float SunAxialTilt = 7.25f;

    private void Start()
    {
        SunObject.transform.rotation = Quaternion.Euler(SunAxialTilt, 0, 0);
    }

    private void Update()
    {
        SunObject.transform.Rotate(RotationVector * Time.deltaTime);
    }
}