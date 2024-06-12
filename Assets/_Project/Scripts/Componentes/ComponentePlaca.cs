using System;
using System.Collections;
using System.Collections.Generic;
using Componentes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
//public class ComponentePlaca : MonoBehaviour, ComponenteBinario, IUsable
public class ComponentePlaca : ComponenteBinario
{
    private ComponenteAnimado _componenteAnimado;

    private void Start()
    {
        // Get script ComponenteAnimado
        _componenteAnimado = GetComponent<ComponenteAnimado>();

        if (_componenteAnimado == null)
        {
            Debug.LogError("No se encontró el script ComponenteAnimado.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el collider pertenece al jugador
        if (other.CompareTag("Player"))
        {
            Encender(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Verificar si el collider pertenece al jugador
        if (other.CompareTag("Player"))
        {
            Encender(false);
        }
    }
}