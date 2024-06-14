using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scrip_Menu : MonoBehaviour
{
    public void Menu()
    {

        Application.Quit();
        Debug.Log("Salir...");
    }
    public void Jugar() {
        Debug.Log("Jugar...");
        SceneManager.LoadScene("SampleScene");
    }

}
