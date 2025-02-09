using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Libreria que permite usar metodos de carga/descarga de escenas

public class SceneChanger : MonoBehaviour
{
    public void SceneLoader(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        Application.Quit(); //Salir de la aplicaci�n, cierra el juego completamente
    }

    [SerializeField] private string Escena;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Escena);

        }
    }
}
