using System;
using System.Collections;
using System.Collections.Generic;
using Componentes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
public class ComponentePlaca : ComponenteBinario
{
    [SerializeField] private string tagAComparar = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el collider pertenece al jugador
        if (other.CompareTag(tagAComparar))
        {
            Encender(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Verificar si el collider pertenece al jugador
        if (other.CompareTag(tagAComparar))
        {
            Encender(false);
        }
    }
}
