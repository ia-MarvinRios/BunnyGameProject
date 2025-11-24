using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Transform _checkpoint;
    [SerializeField] TMP_Text _carrotCountLabel;
    [SerializeField] GameObject _livesContainerUI;
    [SerializeField] GameObject _lifeIconPrefab;
    [SerializeField] int _maxLives = 3;
    [Header("Game Over Events")]
    [SerializeField] UnityEvent _onGameOver;
    int _collectedCarrots;
    int _carrotCount = 0;
    bool _isGamePaused = false;

    public delegate void OnObjectToggleDelegate(GameObject obj);
    public event OnObjectToggleDelegate OnObjectToggle;

    public bool IsGamePaused { get { return _isGamePaused; } }
    public int MaxLives { get { return _maxLives; } }

    private void Awake()
    {
        Instance = this;

        // Event Subscriptions
        PlayerController.OnTakeDamage += UpdateLivesUI;
        PlayerController.OnHealthZero += HandleGameOver;
    }
    private void Start()
    {
        PlayMusic(GetCurrentSceneName());
        CountCarrots();
        SetUpLives();
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
    public void ReloadScene()
    {
        GoToScene(GetCurrentSceneName());
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
    public string GetCurrentSceneName() {
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
            case "Level 3":
                AudioManager.Instance.PlaySoundByName("InGame01");
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
    void SetUpLives()
    {
        if (_livesContainerUI != null && _lifeIconPrefab != null)
        {
            for (int i = 0; i < _maxLives; i++)
            {
                Instantiate(_lifeIconPrefab, _livesContainerUI.transform);
            }
        }
    }
    public void UpdateLivesUI(int currentLives)
    {
        if (_livesContainerUI != null)
        {
            for (int i = 0; i < _livesContainerUI.transform.childCount; i++)
            {
                GameObject lifeIcon = _livesContainerUI.transform.GetChild(i).gameObject;
                lifeIcon.SetActive(i < currentLives);
            }
        }
    }
    void HandleGameOver()
    {
        _onGameOver?.Invoke();
    }
}
