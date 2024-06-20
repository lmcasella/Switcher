using System.Collections;
using UnityEngine;

namespace Componentes
{
    [RequireComponent(typeof(Collider2D))]
    public class ComponenteTeleport : MonoBehaviour
    {
        [Header("Configuración")]
        //[SerializeField] private Transform teleportEntrada;
        [SerializeField] private Transform teleportSalida;
        [SerializeField] private float teleportDelay = 1f;

        private bool canTeleport = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (canTeleport && other.CompareTag("Player"))
            {
                StartCoroutine(TeleportPlayerWithDelay(other.transform));
            }
        }

        private IEnumerator TeleportPlayerWithDelay(Transform player)
        {
            yield return new WaitForSeconds(teleportDelay); // Delay antes de teletransportar
            TeleportPlayer(player);
        }

        private void TeleportPlayer(Transform player)
        {
            if (teleportSalida != null)
            {
                player.position = teleportSalida.position;
                DisableTeleportationTemporarily();
            }
        }

        public void DisableTeleportationTemporarily()
        {
            canTeleport = false;
            Invoke(nameof(EnableTeleportation), 1f); // Deshabilitar teletransportación por 1 segundo
        }

        private void EnableTeleportation()
        {
            canTeleport = true;
        }

        private void OnDrawGizmos()
        {

            if (teleportSalida != null)
            {
                // Dibujar línea de conexión
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, teleportSalida.position);

                // Dibujar punto de salida
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(teleportSalida.position, 0.2f);
            }
        }
    }
}
