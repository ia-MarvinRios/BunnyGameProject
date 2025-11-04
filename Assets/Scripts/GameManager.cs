using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void GoToScene(string sceneName = "MainMenu")
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
