using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Componentes
{
    public class ComponenteSetActive : MonoBehaviour
    {
        [Header("Config")] 
        [SerializeField] private bool detectarAlPresionar = true;
        [SerializeField] private bool detectarAlEncender = true;
        [SerializeField] private bool detectarAlApagar = true;
        
        [Header("Conexión")]
        [SerializeField] private ComponenteBinario interruptor;

        private void OnEnable()
        {
            interruptor.OnEncender += OnEncender;
            interruptor.OnApagar += OnApagar;
            interruptor.OnPresionar += OnPresionar;
        }

        private void OnEncender(object sender, EventArgs e)
        {
            if (!detectarAlEncender) return;
            gameObject.SetActive(true);
        }

        private void OnApagar(object sender, EventArgs e)
        {
            if (!detectarAlApagar) return;
            gameObject.SetActive(false);
        }

        private void OnPresionar(object sender, EventArgs e)
        {
            if (!detectarAlPresionar) return;
            gameObject.SetActive(interruptor.Encendido);
        }

        private void OnValidate() => ValidarComponentes();

        private void Awake() => ValidarComponentes();

        private void ValidarComponentes()
        {
            if (TryGetComponent(out ComponenteBinario componenteBinario))
                interruptor = componenteBinario;    
        }
    }
}