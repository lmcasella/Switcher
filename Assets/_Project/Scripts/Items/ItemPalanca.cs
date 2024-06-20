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
        Vector2.Distance(Jugador.transform.position, componenteAActivar.transform.position);
    
    public override void Utilizar(Inventario inventario)
    {
        if (DistanciaHastaComponente < 1.25f)
        {
            if (componenteAActivar.Encendido)
            {
                inventario.Usuario.Decir(mensajeComponenteUsado, 3);
                return;
            }
            
            componenteAActivar.Habilitar(true);
            componenteAActivar.Encender(true);
            inventario.Usuario.Decir(mensajeAlUsar, 2);
            inventario.AccionSoltarItem();
            Deshabilitar();
        }
        else
            inventario.Usuario.Decir(mensajeAlFallar, 3);
    }

    private void OnDrawGizmosSelected()
    {
        if (!componenteAActivar) return;
        Gizmos.DrawLine(transform.position, componenteAActivar.transform.position);
    }
}