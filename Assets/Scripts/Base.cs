using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Base : MonoBehaviour
{
    [SerializeField] bool firstBase;
    [SerializeField] bool lastBase;
    [SerializeField] private Renderer[] baseColorZones;
    [SerializeField] private PlayableDirector cameraAnimation;
    [SerializeField] private GameObject[] bases;
    public static Action OnSuccesfulLanding;
    public static Action OnSuccesfulCinematic;
    private static int nextBase = 0;
    private bool alreadyUsed;

    private void Start()
    {
        alreadyUsed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && Vector3.Angle(transform.up, collision.gameObject.transform.forward) < 40 && !alreadyUsed && (bases[nextBase] == this.gameObject || firstBase))
        {
            if (!lastBase && !firstBase && !alreadyUsed)
            {
                nextBase++;
                alreadyUsed = true;
            }
            if (!lastBase)
            {
                bases[nextBase].GetComponent<Base>().baseColorZones[0].material.color = Color.yellow;
                bases[nextBase].GetComponent<Base>().baseColorZones[1].material.color = Color.yellow;
                bases[nextBase].tag = "Base";
            }
            if (!gameObject.CompareTag("Base"))
            {
                gameObject.tag = "Base";
            }
            OnSuccesfulLanding();
            if(!firstBase)
            cameraAnimation.gameObject.SetActive(true);
            foreach (Renderer zone in baseColorZones)
            {
                zone.material.color = Color.green;
            }
            StartCoroutine(StartCinematic());
        }
    }

    private IEnumerator StartCinematic()
    {
        while(cameraAnimation.state == PlayState.Playing)
        {
            yield return null;
        }
        OnSuccesfulCinematic();
    }
}
