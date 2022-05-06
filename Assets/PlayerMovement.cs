using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XInputDotNetPure;

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
    private static float gasoline = 100f;
    public static Action<float> OnPropulsorUse;
    public static Action OnDamage;
    private PlayerIndex playerIndexOne = PlayerIndex.One;
    private bool enabled = true;

    private void Awake()
    {
        GamePad.SetVibration(playerIndexOne, 0f, 0f);
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
        ps = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        particle = transform.GetChild(1).gameObject;
        main = ps.main;
        Base.OnSuccesfulLanding += EnableControls;
        Base.OnSuccesfulCinematic += EnableControls;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        if (enabled)
        {
            if (rb)
            {
                rb.AddTorque(cam.gameObject.transform.forward * 5f * -horizontal, ForceMode.Force);
                rb.AddTorque(cam.gameObject.transform.right * 5f * -vertical, ForceMode.Force);
                if (Input.GetAxis("Boost") != 0 && gasoline > 0)
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
                    gasoline -= 2f * Time.deltaTime;
                    OnPropulsorUse?.Invoke(gasoline);
                }
                else
                {
                    if (ps)
                        main.maxParticles = 0;
                    main.loop = false;
                    propulsorON = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base") && Vector3.Angle(transform.forward, collision.gameObject.transform.up) < 40)
        {
            gasoline = 100f;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            OnPropulsorUse?.Invoke(gasoline);
        }
        else
        {
            Debug.Log(collision.gameObject.name);
            main.maxParticles = 0;
            for(int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            }
            StartCoroutine(StartShaking());
            OnDamage?.Invoke();
            gasoline = 0;
            Destroy(GetComponent<Rigidbody>());
        }
    }

    private IEnumerator StartShaking()
    {
        GamePad.SetVibration(playerIndexOne, 1f, 1f);
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
        GamePad.SetVibration(playerIndexOne, 0f, 0f);
    }

    private void EnableControls()
    {
        enabled = !enabled;
    }
}
