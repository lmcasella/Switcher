using System;
using Componentes;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemPalanca : Item
{
    [SerializeField] private ComponenteBinario componenteAActivar;
    [SerializeField] private string mensajeAlUsar = "¡Funcionó!";
    [SerializeField] private string mensajeAlFallar = "Estoy muy lejos...";
    [SerializeField] private string mensajeComponenteUsado = "Ya abrí esta puerta."; 

    private float DistanciaHastaComponente =>
        Vector2.Distance(_jugador.transform.position, componenteAActivar.transform.position);
    
    public override void Utilizar(Inventario inventario)
    {
        if (DistanciaHastaComponente < 1.25f)
        {
            if (componenteAActivar.Encendido)
            {
                inventario.usuario.Decir(mensajeComponenteUsado, 3);
                return;
            }
            
            componenteAActivar.Habilitar(true);
            componenteAActivar.Encender(true);
            inventario.usuario.Decir(mensajeAlUsar, 2);
            inventario.AccionSoltarItem();
            Deshabilitar();
        }
        else
            inventario.usuario.Decir(mensajeAlFallar, 3);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, componenteAActivar.transform.position);
    }
}