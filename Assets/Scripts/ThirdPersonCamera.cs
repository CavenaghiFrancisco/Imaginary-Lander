using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform lookAt;

    private const float yAngleMin = -10f; 
    private const float yAngleMax = 50f;

    private Transform camTransform;
    private Camera cam;
    private float distanceZ = 30f;
    private float distanceY = 5f;
    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 cameraDirection;
    private float camDistance;
    private Vector2 cameraDistanceMinMax = new Vector2(0.5f, 30f);

    private void Start()
    {
        camTransform = this.transform;
        cam = Camera.main;
        cameraDirection = cam.transform.localPosition.normalized;
        camDistance = cameraDistanceMinMax.y;
    }

    private void Update()
    {
        currentX += Input.GetAxis("RightStickHorizontal");
        currentY -= Input.GetAxis("RightStickVertical");

        currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
        CheckCamaraCollision();
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, distanceY, -distanceZ);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);

    }

    private void CheckCamaraCollision()
    {
        Vector3 desiredCameraPosition = transform.TransformPoint(cameraDirection * cameraDistanceMinMax.y);
        RaycastHit hit;
        if(Physics.Linecast(transform.position, desiredCameraPosition,out hit))
        {
            distanceZ = -Mathf.Clamp(hit.distance, cameraDistanceMinMax.y, cameraDistanceMinMax.x);
        }
        else
        {
            distanceZ = -cameraDistanceMinMax.y;
        }
        camTransform.position = cameraDirection * distanceZ;
    }
}
