using System;
using System.Collections;
using UnityEngine;

namespace Componentes
{
    public class ComponenteBinario : MonoBehaviour, IInterruptor
    {
        public event EventHandler OnEncender;
        public event EventHandler OnApagar;
        public event EventHandler OnPresionar;

        [Header("Configuración")] 
        [SerializeField] protected bool encendidoPorDefecto = false;

        [SerializeField] protected bool invertir = false;
        [SerializeField] protected float delayEncender = 0.3f;
        [SerializeField] protected float delayApagar = 0.3f;

        public bool Encendido => invertir ? !_encendido : _encendido;

        public bool Habilitado { get; private set; } = true;

        private bool _encendido;

        private Coroutine _corrutinaAnterior;

        public void Encender(bool valor)
        {
            if (!Habilitado) return;

            if (_corrutinaAnterior != null) StopCoroutine(_corrutinaAnterior);
            _corrutinaAnterior = StartCoroutine(EstadoConDelay(valor));
        }

        public void Habilitar(bool valor)
        {
            Habilitado = valor;
        }

        protected virtual void EstadoEncendido()
        {
            OnEncender?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void EstadoApagado()
        {
            OnApagar?.Invoke(this, EventArgs.Empty);
        }

        protected virtual IEnumerator EstadoConDelay(bool valor)
        {
            _encendido = valor;
            OnPresionar?.Invoke(this, EventArgs.Empty);
            yield return new WaitForSeconds(Encendido ? delayEncender : delayApagar);
            if (Encendido) EstadoEncendido();
            else EstadoApagado();
        }

        private void Start()
        {
            Encender(encendidoPorDefecto);
        }
    }
}