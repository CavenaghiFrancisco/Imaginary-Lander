using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainBase : MonoBehaviour
{
    public static Action OnCheckWinCondition;

    private void OnCollisionEnter(Collision collision)
    {
        OnCheckWinCondition?.Invoke();
    }
}
