using System;
using UnityEngine;

namespace Componentes
{
    [RequireComponent(typeof(AudioSource), typeof(ComponenteBinario))]
    public class ComponenteSFX : MonoBehaviour
    {
        [Header("Clips de sonido")] 
        [Tooltip("Se reproduce cuando el usuario presiona el botón y el estado previo era apagado.")]
        [SerializeField] private AudioClip sfxPresionarEncender;
        [Tooltip("Se reproduce cuando el usuario presiona el botón y el estado previo era encendido.")]
        [SerializeField] private AudioClip sfxPresionarApagar;
        [Tooltip("Se reproduce una vez el componente se enciende.")]
        [SerializeField] private AudioClip sfxEncendido; 
        [Tooltip("Se reproduce una vez el componente se apaga.")]
        [SerializeField] private AudioClip sfxApagado;

        private ComponenteBinario _componente;
        private AudioSource _audioSource;

        private void Awake()
        {
            _componente = GetComponent<ComponenteBinario>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnValidate()
        {
            if (TryGetComponent(out ComponenteBinario componente))
                _componente = componente;
            
            if (TryGetComponent(out AudioSource audioSource))
            {
                _audioSource = audioSource;
                _audioSource.spatialBlend = 1;
            }
        }

        private void OnEnable()
        {
            _componente.OnEncender += (sender, args) => InterruptorEncendido();
            _componente.OnApagar += (sender, args) => InterruptorApagado();
            _componente.OnPresionar += (sender, args) => InterruptorPresionado();            
        }

        private void InterruptorEncendido()
        {
            if (!sfxEncendido) return;
            _audioSource.PlayOneShot(sfxEncendido);
        }

        private void InterruptorApagado()
        {
            if (!sfxApagado) return;
            _audioSource.PlayOneShot(sfxApagado);
        }

        private void InterruptorPresionado()
        {
            if (sfxPresionarEncender && _componente.Encendido) _audioSource.PlayOneShot(sfxPresionarEncender);
            if (sfxPresionarApagar && !_componente.Encendido) _audioSource.PlayOneShot(sfxPresionarApagar);
        }
    }
}