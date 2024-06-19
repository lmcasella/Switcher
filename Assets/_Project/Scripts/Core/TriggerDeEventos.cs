using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDeEventos : MonoBehaviour
{
    public UnityEvent OnTriggerEnterAction;
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnterAction.Invoke();
    }
}
