using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider _musicSlider;

    private void Start()
    {
        SetUpMusicSlider();
    }

    void SetUpMusicSlider()
    {
        _musicSlider.value = AudioManager.Instance.MusicVolume;
        _musicSlider.onValueChanged.AddListener((float a) =>
        {
            AudioManager.Instance.MusicVolume = _musicSlider.value;
        });
    }
}
