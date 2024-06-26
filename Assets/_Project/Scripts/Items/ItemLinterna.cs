using System;
using Componentes;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class ItemLinterna : Item
{
    [SerializeField] private ComponenteBinario componenteAActivar;
    [SerializeField] private string mensajeAlAgarrar = "Ahora puedo ver...";
    [SerializeField] private string mensajeComponenteUsado = "Por fín, ya hay luz";

    private Light2D _linterna;

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
            inventario.Usuario.Decir(mensajeAlAgarrar, 2);
            inventario.AccionSoltarItem();
            Deshabilitar();
        }
        else
        {
            //inventario.Usuario.Decir(mensajeAlFallar, 3);
        }
    }

    public override void Usar(ControlJugador usuario)
    {
        base.Usar(usuario);
        _linterna = GetComponentInChildren<Light2D>();
        _linterna.falloffIntensity = 0.4f;
    }

    private void OnDrawGizmosSelected()
    {
        if (!componenteAActivar) return;
        Gizmos.DrawLine(transform.position, componenteAActivar.transform.position);
    }
}