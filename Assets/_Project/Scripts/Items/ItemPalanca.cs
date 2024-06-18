using System;
using Componentes;
using UnityEngine;

public class ItemPalanca : Item
{
    [SerializeField] private ComponenteBinario componenteAActivar;

    private float DistanciaHastaComponente =>
        Vector2.Distance(_jugador.transform.position, componenteAActivar.transform.position);
    
    public override void Utilizar(Inventario inventario)
    {
        if (DistanciaHastaComponente < 1.25f)
        {
            componenteAActivar.Habilitar(true);
            componenteAActivar.Encender(true);
            inventario.AccionSoltarItem();
            Deshabilitar();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, componenteAActivar.transform.position);
    }
}