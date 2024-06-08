using System;
using UnityEngine;

namespace Componentes
{
    public class ComponenteInterruptor : ComponenteBinario, IUsable
    {
        public void Usar()
        {
            Encender(!Encendido);
        }
    }
}