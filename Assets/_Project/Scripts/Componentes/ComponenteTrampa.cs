using System;
using UnityEngine;

namespace Componentes
{
    [RequireComponent(typeof(Collider2D))]
    public class ComponenteTrampa : ComponenteBinario
    {
        [SerializeField] private ComponenteBinario interruptor;
        [SerializeField] private Color colorEncendido = Color.red;
        [SerializeField] private Color colorApagado = Color.gray;
        
        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            interruptor.OnEncender += (sender, args) => EstadoEncendido();
            interruptor.OnApagar += (sender, args) => EstadoApagado();
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void EstadoEncendido()
        {
            base.EstadoEncendido();
            _collider.enabled = true;
            _spriteRenderer.color = colorEncendido;
        }

        protected override void EstadoApagado()
        {
            base.EstadoApagado();
            _collider.enabled = false;
            _spriteRenderer.color = colorApagado;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.DealDamage();
                EstadoApagado();
                if(interruptor) Encender(interruptor.Encendido);
                else Encender(encendidoPorDefecto);
            }
        }
    }
}