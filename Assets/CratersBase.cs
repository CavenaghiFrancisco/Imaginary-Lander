using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratersBase : MonoBehaviour
{
    private GameObject savedAstronauts;
    private bool collected;
    public static Action OnSuccesfulLanding;

    private void Awake()
    {
        savedAstronauts = GameObject.FindGameObjectWithTag("Saved");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collected)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collected = true;
                OnSuccesfulLanding();
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                for(int i = 0; i < savedAstronauts.transform.childCount; i++)
                {
                    if (!savedAstronauts.transform.GetChild(i).gameObject.activeSelf)
                    {
                        savedAstronauts.transform.GetChild(i).gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}
