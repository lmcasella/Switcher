using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Componentes
{
    public class ComponenteRespawner : ComponenteBinario
    {
        [SerializeField] private ControlJugador.NumeroJugador jugadorARespawnear;
        [SerializeField] private ComponenteBinario interruptor;
        
        private ControlJugador _jugadorGuardado;

        private void Awake()
        {
            Guardar();
        }

        private void OnEnable()
        {
            interruptor.OnEncender += (a, b) => Encender(interruptor.Encendido);
            interruptor.OnApagar += (a, b) => Encender(interruptor.Encendido);
        }

        protected override void EstadoEncendido()
        {
            base.EstadoEncendido();
            EjecutarRespawn();
        }

        private void EjecutarRespawn() => StartCoroutine(Respawnear());
        
        private IEnumerator Respawnear()
        {
            if (!Encendido || !_jugadorGuardado.IsDead) yield break;
            yield return new WaitForSeconds(3);
            _jugadorGuardado.Revivir();
            _jugadorGuardado.transform.position = transform.position;
        }

        private void Guardar()
        {
            _jugadorGuardado = EncontrarJugador(jugadorARespawnear);
            _jugadorGuardado.OnDeath += (j) => EjecutarRespawn();
        }

        private ControlJugador EncontrarJugador(ControlJugador.NumeroJugador numero)
        {
            return FindObjectsOfType<ControlJugador>()
                .Where(j => j.numeroJugador == numero)
                .FirstOrDefault();
        }
    }
}