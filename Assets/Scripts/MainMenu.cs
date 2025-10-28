using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Overworld";

    public void Play()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
        else
            Debug.LogError("?? No se ha asignado el nombre de la escena en el Inspector.");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("?? La aplicaci�n se cerrar�a (solo funciona en compilaci�n).");
    }
}
