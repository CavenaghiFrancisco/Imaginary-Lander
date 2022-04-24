using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform lookAt;

    private const float yAngleMin = 0f; 
    private const float yAngleMax = 50f;

    private Transform camTransform;
    private Camera cam;
    private float distanceZ = 20f;
    private float distanceY = 5f;
    private float currentX = 0f;
    private float currentY = 0f;
    private float sensivityX = 4f;
    private float sensivityY = 1f;

    private void Start()
    {
        camTransform = this.transform;
        cam = Camera.main;
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, distanceY, -distanceZ);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
