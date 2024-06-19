using Componentes;
using UnityEngine;

public class ItemLlaveInglesa : Item
{
    [SerializeField] private ComponenteBinario componenteAActivar;
    [SerializeField] private string mensajeAlUsar = "¡Funcionó!", mensajeAlFallar = "Debo reparar algo...";

    private float DistanciaHastaComponente =>
        Vector2.Distance(_jugador.transform.position, componenteAActivar.transform.position);
    
    public override void Utilizar(Inventario inventario)
    {
        if (DistanciaHastaComponente < 1.25f)
        {
            componenteAActivar.Habilitar(true);
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