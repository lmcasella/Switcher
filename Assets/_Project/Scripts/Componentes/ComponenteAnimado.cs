using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Componentes
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ComponenteAnimado : MonoBehaviour
    {
        [Header("Sprites")] 
        [SerializeField] private Sprite spriteActivo;
        [SerializeField] private Sprite spriteApagado;

        [Header("Config")] 
        [SerializeField] private bool detectarAlPresionar = true;
        [SerializeField] private bool detectarAlEncender = true;
        [SerializeField] private bool detectarAlApagar = true;

        [Header("Conexion")]
        [SerializeField] private ComponenteBinario interruptorAValidar;

        private SpriteRenderer _sprite;

        private void OnEnable()
        {
            if (detectarAlPresionar)
                interruptorAValidar.OnPresionar += EstablecerSpriteAdecuado;

            if (detectarAlEncender)
                interruptorAValidar.OnEncender += EstablecerSpriteAdecuado;

            if (detectarAlApagar)
                interruptorAValidar.OnApagar += EstablecerSpriteAdecuado;
        }

        private void OnDisable()
        {
            interruptorAValidar.OnPresionar -= EstablecerSpriteAdecuado;
            interruptorAValidar.OnEncender -= EstablecerSpriteAdecuado;
            interruptorAValidar.OnApagar -= EstablecerSpriteAdecuado;
        }

        private void EstablecerSpriteAdecuado(object sender, EventArgs e)
        {
            ComponenteBinario c = (ComponenteBinario)sender;
            EstablecerSpriteAdecuado(c.Encendido);
        }

        private void EstablecerSpriteAdecuado(bool valor)
        {
            _sprite.sprite = valor ? spriteActivo : spriteApagado;
        }

        private void OnValidate()
        {
            if (TryGetComponent(out ComponenteBinario interruptor))
                interruptorAValidar = interruptor;

            if (TryGetComponent(out SpriteRenderer sprite))
                _sprite = sprite;

            if (interruptorAValidar)
                EstablecerSpriteAdecuado(interruptorAValidar.Encendido);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if(interruptorAValidar)
                Gizmos.DrawLine(transform.position, interruptorAValidar.transform.position);
        }
    }
}
