using System;
using UnityEngine;

namespace Dialogo
{
    
    public class TriggerDialogo : MonoBehaviour
    {
        [SerializeField] private string mensaje = "Hola!";
        [SerializeField] private float delay = 3;
        [SerializeField] private bool sacarAlSalir = false;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ControlJugador jugador))
            {
                jugador.Decir(mensaje, delay);
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