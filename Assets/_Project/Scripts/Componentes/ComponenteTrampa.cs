using System;
using UnityEngine;

namespace Componentes
{
    [RequireComponent(typeof(Collider2D))]
    public class ComponenteTrampa : ComponenteBinario
    {
        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void EstadoEncendido()
        {
            base.EstadoEncendido();
            _collider.enabled = true;
            _spriteRenderer.color = Color.red;
        }

        protected override void EstadoApagado()
        {
            base.EstadoApagado();
            _collider.enabled = false;
            _spriteRenderer.color = Color.gray;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
                damageable.DealDamage();
            EstadoApagado();
            Encender(true);
            Debug.Log("Dañado!");
        }
    }
}