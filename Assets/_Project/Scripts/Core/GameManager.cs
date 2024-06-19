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
    public Scene escenaMenuPrincipal;
    public Scene escenaDeDerrota;
    public Scene escenaDeVictoria;
    public Scene escenaSuperacionDeNivel;
    public Scene[] niveles;

    private int _nivelActual;

    /// <summary>
    /// Carga la escena de victoria del juego por haber completado todos los niveles.
    /// </summary>
    public static void GanarJuego()
    {
        SceneManager.LoadScene(Instance.escenaDeVictoria.name);
    }
    
    /// <summary>
    /// Carga la escena de superación de nivel, en la cual hay opciones para volver al menú o continuar al próximo.
    /// </summary>
    public static void SuperarNivel()
    {
        SceneManager.LoadScene(Instance.escenaSuperacionDeNivel.name);
        Instance.nivelesSuperados = Instance._nivelActual;
    }
    
    /// <summary>
    /// Carga la escena de superación de nivel, en la cual hay una opción para volver al menú o reintentar.
    /// </summary>
    public static void Perder()
    {
        SceneManager.LoadScene(Instance.escenaDeDerrota.name);
    }
    
    /// <summary>
    /// Carga un nivel determinado.
    /// </summary>
    /// <param name="nivel"></param>
    public static void CargarNivel(int nivel)
    {
        Instance._nivelActual = nivel;
        SceneManager.LoadScene(Instance.niveles[nivel].name);
    }

    /// <summary>
    /// Carga el siguiente nivel del actual.
    /// </summary>
    public static void CargarSiguienteNivel()
    {
        if (Instance.niveles.Length >= Instance._nivelActual)
        {
            GanarJuego();
            return;
        }
        CargarNivel(Instance._nivelActual + 1);
    }
    
    /// <summary>
    /// Carga la escena del menú principal.
    /// </summary>
    public static void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene(Instance.escenaMenuPrincipal.name);
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
