using System.Collections;
using UnityEngine;

namespace Componentes
{
    [RequireComponent(typeof(Collider2D))]
    public class ComponenteTeleport : MonoBehaviour
    {
        [Header("Configuración")]
        //[SerializeField] private Transform teleportEntrada;
        //[SerializeField] private Transform teleportSalida;
        [SerializeField] private ComponenteTeleport teleporterAsociado;
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
            if (teleporterAsociado != null)
            {
                player.position = teleporterAsociado.transform.position;
                teleporterAsociado.DisableTeleportationTemporarily();
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

            if (teleporterAsociado != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, teleporterAsociado.transform.position);

                // Dibujar punto de salida
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(teleporterAsociado.transform.position, 0.2f);
            }
        }
    }
}
