using System.Linq;
using Componentes;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Items
{
    public class ItemMatafuegos : Item
    {
        
        [SerializeField] private string mensajeAlUsar = "¡Me quedan {0} usos!";
        [SerializeField] private string mensajeAlFallar = "Tengo que apagar el fuego...";
        [SerializeField] private int usos = 4;

        private float DistanciaHastaComponente =>
            Vector2.Distance(_jugador.transform.position, TrampaMasCercana().transform.position);

        private ComponenteBinario TrampaMasCercana()
        {
            return GameObject.FindGameObjectsWithTag("Fuego")
                .OrderBy(t => Vector2.Distance(t.transform.position, _jugador.transform.position))
                .FirstOrDefault()
                .GetComponent<ComponenteBinario>();
        }
        
        public override void Utilizar(Inventario inventario)
        {
            if (DistanciaHastaComponente < 1.5f)
            {
                Destroy(TrampaMasCercana().gameObject);
                usos--;
                inventario.usuario.Decir(string.Format(mensajeAlUsar, usos), 2);
                if (usos > 0) return; 
                inventario.AccionSoltarItem();
                inventario.usuario.Decir("Ya no queda más...", 2);
                Deshabilitar();
            }
            inventario.usuario.Decir(mensajeAlFallar, 3);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, TrampaMasCercana().transform.position);
        }
    }
}