using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform followed;
    [SerializeField] private float distance;

    void Start()
    {
        distance = transform.position.z - followed.position.z;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, followed.position.z + distance);
    }
}
