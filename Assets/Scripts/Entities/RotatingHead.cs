using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingHead : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * (Random.value * 10f) * Time.deltaTime);
    }
}
