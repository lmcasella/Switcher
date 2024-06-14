using System;
using UnityEngine;

namespace Componentes {
    public class ComponenteOR : ComponenteBinario
    {
        [SerializeField] private ComponenteBinario A, B;

        private void OnEnable()
        {
            if (!(A && B)) return;
            A.OnEncender += RecibirConexion;
            A.OnApagar += RecibirConexion;
            B.OnEncender += RecibirConexion;
            B.OnApagar += RecibirConexion;
        }

        private void OnDisable()
        {
            A.OnEncender -= RecibirConexion;
            A.OnApagar -= RecibirConexion;
            B.OnEncender -= RecibirConexion;
            B.OnApagar -= RecibirConexion;
        }

        private void RecibirConexion(object sender, EventArgs eventArgs)
        {
            Encender(A.Encendido || B.Encendido);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!(A && B)) return;
            MostrarConexionDeInterruptor(A);
            MostrarConexionDeInterruptor(B);
        }

        private void MostrarConexionDeInterruptor(ComponenteBinario interruptor)
        {
            Gizmos.color = interruptor.Encendido ? Color.green : Color.red;
            Gizmos.DrawLine(interruptor.transform.position, transform.position);
        }
        #endif
    }
}