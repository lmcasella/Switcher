using Componentes;
using UnityEngine;

public class ItemLlaveInglesa : Item
{
    [SerializeField] private ComponenteBinario componenteAActivar;
    [SerializeField] private string mensajeAlUsar = "¡Funcionó!";
    [SerializeField] private string mensajeAlFallar = "Debo reparar algo...";
    [SerializeField] private string mensajeComponenteUsado = "Ya reparé esto.";
    private float DistanciaHastaComponente =>
        Vector2.Distance(_jugador.transform.position, componenteAActivar.transform.position);
    
    public override void Utilizar(Inventario inventario)
    {
        if (DistanciaHastaComponente < 1.25f)
        {
            if (componenteAActivar.Habilitado)
            {
                inventario.usuario.Decir(mensajeComponenteUsado, 3);
                return;
            }
            
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