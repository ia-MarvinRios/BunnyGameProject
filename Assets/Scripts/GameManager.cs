using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Transform _checkpoint;
    [SerializeField] TMP_Text _carrotCountLabel;
    int _collectedCarrots;
    int _carrotCount = 0;
    bool _isGamePaused = false;

    public delegate void OnObjectToggleDelegate(GameObject obj);
    public event OnObjectToggleDelegate OnObjectToggle;

    public bool IsGamePaused { get { return _isGamePaused; } }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayMusic(GetCurrentSceneName());
        CountCarrots();
    }

    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;

        if (_isGamePaused)
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
            case "Level 2":
                AudioManager.Instance.PlaySoundByName("InGame02");
                break;
            default:
                break;
        }
    }

    // --- Game Managment Methods ---
    void CountCarrots()
    {
        foreach (var carrot in FindObjectsByType<Moneda>(FindObjectsSortMode.None))
        {
            if (!carrot.IsGolden)
                _carrotCount++;
        }

        UpdateCarrotCount(0);
    }
    /// <summary>
    /// Updates the carrot count UI by the given amount.
    /// </summary>
    /// <param name="plusAmmount"></param>
    public void UpdateCarrotCount(int plusAmmount)
    {
        if (_carrotCountLabel != null)
        {
            _collectedCarrots += plusAmmount;
            _carrotCountLabel.text = $"{_collectedCarrots.ToString()}/{_carrotCount.ToString()}";
        }
        else
        {
            Debug.LogWarning("Carrot Count Label is not assigned in the GameManager.");
        }
    }
}
