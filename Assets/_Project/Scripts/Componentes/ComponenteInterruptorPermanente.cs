using UnityEngine;

namespace Componentes
{
    [RequireComponent(typeof(Collider2D))]
    public class ComponenteInterruptorPermanente : ComponenteBinario, IUsable
    {
        public void Usar()
        {
            Encender(!Encendido);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}