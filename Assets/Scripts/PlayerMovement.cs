using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XInputDotNetPure;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Cinemachine.CinemachineFreeLook cineCam;
    private Rigidbody rb;
    private Collider col;
    private Camera cam;
    private float horizontal;
    private float vertical;
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
            if (Input.GetJoystickNames()[0] != "")
            {
                if (enabled)
                {
                    cineCam.m_YAxis.m_InputAxisName = "RightStickVertical";
                    cineCam.m_XAxis.m_InputAxisName = "RightStickHorizontal";
                    horizontal = Input.GetAxis("Horizontal");
                    vertical = Input.GetAxis("Vertical");
                }
            }
            else
            {
                if (enabled)
                {
                    cineCam.m_YAxis.m_InputAxisName = "Mouse Y";
                    cineCam.m_XAxis.m_InputAxisName = "Mouse X";
                    horizontal = Input.GetAxis("KeyboardHorizontal");
                    vertical = Input.GetAxis("KeyboardVertical");
                }
            }
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
                if (Input.GetAxis("Boost") != 0 || Input.GetAxis("KeyboardBoost") != 0 && gasoline > 0)
                {
                    if (!propulsorON)
                    {
                        if (ps)
                        {
                            main.maxParticles = 50;
                            main.loop = true;
                            ps.Play();
                        }
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
                    {
                        main.maxParticles = 0;
                        main.loop = false;
                    }
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
            if(ps)
            main.maxParticles = 0;
            for(int i = 0; i < transform.GetChild(1).childCount; i++)
            {
                transform.GetChild(1).transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            }
            StartCoroutine(StartShaking());
            gasoline = 0;
            Destroy(rb);
            Destroy(col);
            OnPropulsorUse?.Invoke(gasoline);
            OnDamage?.Invoke();
        }
    }

    private IEnumerator StartShaking()
    {
        GamePad.SetVibration(playerIndexOne, 1f, 1f);
        yield return new WaitForSeconds(0.5f);
        enabled=false;
        GamePad.SetVibration(playerIndexOne, 0f, 0f);
    }

    private void EnableControls()
    {
        enabled = !enabled;
    }

    private void OnDestroy()
    {
        Base.OnSuccesfulLanding -= EnableControls;
        Base.OnSuccesfulCinematic -= EnableControls;
    }
}
