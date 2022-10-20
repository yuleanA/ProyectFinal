using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class botton : MonoBehaviour
{
    public int numeroEscena;
    public void iniciar()
    {
        SceneManager.LoadScene(numeroEscena);
    }

    public void salir()
    {
        Application.Quit();

    }
}
