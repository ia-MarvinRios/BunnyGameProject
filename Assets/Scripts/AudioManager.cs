using UnityEngine;
using UnityEngine.Audio;

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
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource _musicSource;
    [SerializeField] GameAudio[] _soundsLibrary;

    public float MusicVolume { get { return GetChannelVolume("Music"); } set { SetChannelVolume("Music", value); } }

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

    public void SetChannelVolume(string mixerChannel, float linearVolume) // valor 0.0 a 1.0
    {
        float volumeInDb = Mathf.Log10(Mathf.Clamp(linearVolume, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(mixerChannel, volumeInDb);
    }
    public float GetChannelVolume(string mixerChannel)
    {
        float volumeInDb;
        mixer.GetFloat(mixerChannel, out volumeInDb);
        float linearVolume = Mathf.Pow(10f, volumeInDb / 20f);
        return linearVolume;
    }
}
