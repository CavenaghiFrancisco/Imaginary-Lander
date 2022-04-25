using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Camera cam;
    private float horizontal;
    private float vertical;
    private ParticleSystem ps;
    private GameObject particle;
    private ParticleSystem.MainModule main;
    private bool propulsorON;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
        ps = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        particle = transform.GetChild(1).gameObject;
        main = ps.main;
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
        transform.RotateAround(cam.gameObject.transform.right, -vertical * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Boost") != 0 || Input.GetAxis("Jump") != 0)
        {
            if (!propulsorON)
            {
                main.maxParticles = 50;
                main.loop = true;
                ps.Play();
                propulsorON = true;
            }
            rb.AddRelativeForce(Vector3.forward * 15);
            particle.SetActive(true);

        }
        else
        {
            main.maxParticles = 0;
            main.loop = false;
            propulsorON = false;
        }
    }
}
