using System;
using UnityEngine;

namespace Componentes
{
    [RequireComponent(typeof(Collider2D))]
    public class ComponentePuerta : ComponenteBinario
    {
        public ComponenteBinario interruptor;
        private Collider2D _collider;

        protected override void EstadoEncendido()
        {
            base.EstadoEncendido();
            AplicarEstadoDeColision();
        }

        protected override void EstadoApagado()
        {
            base.EstadoApagado();
            AplicarEstadoDeColision();
        }

        private void AplicarEstadoDeColision() => _collider.enabled = !Encendido;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            interruptor.OnEncender += OnInterruptorEncendido;
            interruptor.OnApagar += OnInterruptorApagado;
        }

        private void OnDisable()
        {
            interruptor.OnEncender -= OnInterruptorEncendido;
            interruptor.OnApagar -= OnInterruptorApagado;
        }

        private void OnInterruptorEncendido(object sender, EventArgs e)
        {
            Encender(true);
        }

        private void OnInterruptorApagado(object sender, EventArgs e)
        {
            Encender(false);
        }

        private void OnDrawGizmos()
        {
            if (!interruptor) return;
            Gizmos.color = interruptor.Encendido ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, interruptor.transform.position);
        }
    }
}