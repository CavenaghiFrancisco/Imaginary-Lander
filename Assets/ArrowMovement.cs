using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;
    [SerializeField] private float speed;

    private void Start()
    {
        pointA = transform.position;
        pointB = new Vector3(transform.position.x, transform.position.y + 6, transform.position.z);
    }

    void Update()
    {
        transform.RotateAround(Vector3.up, 1f * Time.deltaTime);
        transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time / speed, 1));
    }
}
