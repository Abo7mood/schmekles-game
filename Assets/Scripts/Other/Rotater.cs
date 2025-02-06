using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] float _speed;
    float z;
    void Update()
    {
        z += Time.deltaTime*_speed;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);

        if (transform.localEulerAngles.z > 360)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
