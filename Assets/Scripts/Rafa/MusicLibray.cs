using UnityEngine;

[System.Serializable]
public struct Musictrack
{
    public string trackName;
    public AudioClip Clip;
}

public class MusicLibray : MonoBehaviour
{
    public Musictrack[] tracks;

    public AudioClip GetClipFromName (string trackName)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == trackName)
            {
                return track.Clip;
            }
        }
        return null;
    }
    
}
