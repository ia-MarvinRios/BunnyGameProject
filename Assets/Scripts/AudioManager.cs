using UnityEngine;

[System.Serializable]
public struct GameAudio
{
    public enum AudioType
    {
        SFX,
        Music
    }

    public AudioClip Clip;
    public AudioType Type;
    public string Name;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] bool _playMusicOnLoop = true;
    [SerializeField] AudioSource _musicSource;
    [SerializeField] GameAudio[] _soundsLibrary;

    private void Awake()
    {
        Instance = this;
        CheckMusicSource();
    }

    void CheckMusicSource() {
        if (_musicSource == null) {
            _musicSource = gameObject.GetComponent<AudioSource>();
        }
        else if (gameObject.GetComponent<AudioSource>() == null)
        {
            Debug.LogWarning("AudioManager: No AudioSource component found on the GameObject. Music playback may not work correctly.");
        }

        _musicSource.loop = _playMusicOnLoop;
    }

    public void PlaySoundByName(string name)
    {
        foreach (var sound in _soundsLibrary)
        {
            if (sound.Name == name)
            {
                switch (sound.Type)
                {
                    case GameAudio.AudioType.SFX:
                        AudioSource.PlayClipAtPoint(sound.Clip, Camera.main.transform.position);
                        break;
                    case GameAudio.AudioType.Music:
                        _musicSource.clip = sound.Clip;
                        _musicSource.Play();
                        break;
                }
                break;
            }
        }
    }
}
