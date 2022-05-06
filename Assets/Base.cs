using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Base : MonoBehaviour
{
    [SerializeField] Renderer[] baseColorZones;
    [SerializeField] PlayableDirector cameraAnimation;
    public static Action OnSuccesfulLanding;
    public static Action OnSuccesfulCinematic;
    private bool alreadyUsed = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && Vector3.Angle(transform.up, collision.gameObject.transform.forward) < 40 && !alreadyUsed)
        {
            alreadyUsed = true;
            OnSuccesfulLanding();
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
