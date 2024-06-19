using UnityEngine;

/// <summary>
/// Instancia que se comunica con el singleton del GameManager.
/// </summary>
public class GameManagerTriggerer : MonoBehaviour
{
    public void CompletarJuego() => GameManager.GanarJuego();
    public void SuperarNivel() => GameManager.SuperarNivel();
    public void Perder() => GameManager.Perder();
    public void CargarNivel(int nivel) => GameManager.CargarNivel(nivel);
    public void SiguienteNivel() => GameManager.CargarSiguienteNivel();
    public void Jugar() => GameManager.IniciarJuego();
    public void MenuPrincipal() => GameManager.IrAlMenuPrincipal();
    public void SalirDelJuego() => GameManager.CerrarJuego();
    public void Reiniciar() => GameManager.ReiniciarEscena();
}