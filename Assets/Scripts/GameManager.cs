using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Transform _checkpoint;
    bool isGamePaused = false;

    public delegate void OnObjectToggleDelegate(GameObject obj);
    public event OnObjectToggleDelegate OnObjectToggle;

    public bool IsGamePaused { get { return isGamePaused; } }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayMusic(GetCurrentSceneName());
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void GoToScene(string sceneName = "MainMenu")
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void ToggleGameObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
        OnObjectToggle?.Invoke(obj);
    }
    public void QuitApp()
    {
        Application.Quit();
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                                      .GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call<bool>("moveTaskToBack", true);
    }
    public void RespawnPlayer(GameObject player){
        player.transform.position = _checkpoint.position;
    }
    public void RespawnPlayer(GameObject player, Transform checkpoint){
        player.transform.position = checkpoint.position;
    }
    string GetCurrentSceneName() {
        return SceneManager.GetActiveScene().name;
    }
    void PlayMusic(string sceneName)
    {
        switch (sceneName)
        {
            case "Menu":
                AudioManager.Instance.PlaySoundByName("MainMenu");
                break;
            case "Level":
                AudioManager.Instance.PlaySoundByName("InGame01");
                break;
            default:
                break;
        }
    }
}
