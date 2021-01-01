using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 cameraOffset;
    public float smoothing = 0.5f;
    Vector3 velocity = Vector3.right;

    private void Start()
    {
        cameraOffset = transform.position - targetObject.transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothing);
        //transform.LookAt(targetObject);
    }
}