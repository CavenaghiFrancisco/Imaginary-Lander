using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Camera cam;
    private float horizontal;
    private float vertical;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        transform.RotateAround(cam.gameObject.transform.forward, -horizontal * Time.deltaTime);
        transform.RotateAround(cam.gameObject.transform.right, vertical * Time.deltaTime);

        if (Input.GetAxis("Boost") != 0)
        {
            rb.AddRelativeForce(Vector3.forward * 3000 * Time.deltaTime, ForceMode.Force);
        }
    }
}
