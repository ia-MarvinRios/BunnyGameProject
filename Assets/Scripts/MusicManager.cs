using System.Collections;
using Unity.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private MusicLibray musicLibray;
    [SerializeField]
    private AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayMusic(string trackName, float fadeduration = 005f)
    {
        StartCoroutine(AnimateMusicCrossfade (musicLibray.GetClipFromName (trackName), fadeduration));
    }

    IEnumerator AnimateMusicCrossfade ( AudioClip nexttrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null; 
        }
        musicSource.clip = nexttrack;
        musicSource.Play();

        percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }


    }


}
