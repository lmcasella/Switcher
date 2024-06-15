using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogo
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerDialogo : MonoBehaviour
    {
        [SerializeField] private string mensaje = "Hola!";
        [SerializeField] private float delay = 3;
        [SerializeField] private bool sacarAlSalir = false;
        [SerializeField] private bool destruir = false;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ControlJugador jugador))
            {
                jugador.Decir(mensaje, delay);
                if (destruir)
                    Destroy(gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out ControlJugador jugador) && sacarAlSalir)
            {
                jugador.Decir("", 0);
            }
        }
    }
}