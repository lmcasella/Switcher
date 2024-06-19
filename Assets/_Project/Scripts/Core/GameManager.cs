using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Configuración")] 
    [SerializeField] private GameObject prefabBotonUI;
    public int nivelesSuperados = 0;

    [Header("Escenas")] 
    [Scene] public string escenaMenuPrincipal;
    [Scene] public string escenaDeDerrota;
    [Scene] public string escenaDeVictoria;
    [Scene] public string escenaSuperacionDeNivel;
    [Scene] public string[] niveles;

    private int _nivelActual;

    public static bool IsJuegoCompleto => Instance.nivelesSuperados >= Instance.niveles.Length;

    /// <summary>
    /// Carga la escena de victoria del juego por haber completado todos los niveles.
    /// </summary>
    public static void GanarJuego()
    {
        SceneManager.LoadScene(Instance.escenaDeVictoria);
    }
    
    /// <summary>
    /// Carga la escena de superación de nivel, en la cual hay opciones para volver al menú o continuar al próximo.
    /// </summary>
    public static void SuperarNivel()
    {
        Instance.nivelesSuperados = Instance._nivelActual + 1;
        if (IsJuegoCompleto)
        {
            GanarJuego();
            return;
        }
        
        SceneManager.LoadScene(Instance.escenaSuperacionDeNivel);
    }
    
    /// <summary>
    /// Carga la escena de derrota, en la cual hay una opción para volver al menú o reintentar.
    /// </summary>
    public static void Perder()
    {
        SceneManager.LoadScene(Instance.escenaDeDerrota);
    }
    
    /// <summary>
    /// Carga un nivel determinado.
    /// </summary>
    /// <param name="nivel"></param>
    public static void CargarNivel(int nivel)
    {
        Instance._nivelActual = nivel;
        SceneManager.LoadScene(Instance.niveles[nivel]);
    }

    /// <summary>
    /// Carga el siguiente nivel del actual.
    /// </summary>
    public static void CargarSiguienteNivel()
    {
        CargarNivel(Instance._nivelActual + 1);
    }

    /// <summary>
    /// Carga el último nivel que se jugó, como empieza en 0, la primera vez que se abra el juego siempre va a cagar el primer nivel
    /// </summary>
    public static void IniciarJuego()
    {
        if (IsJuegoCompleto)
        {
            SuperarNivel();
            return;
        }
        CargarNivel(Instance.nivelesSuperados);
    }
    
    /// <summary>
    /// Carga la escena del menú principal.
    /// </summary>
    public static void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene(Instance.escenaMenuPrincipal);
    }
    
    public static void CerrarJuego()
    {
        Application.Quit();
    }

    public static void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
