using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateYUp : MonoBehaviour
{
    public float angleSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(angleSpeed * Time.deltaTime, Vector3.up);
    }
}
