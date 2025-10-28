using UnityEngine;


[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}

public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundeffects;

    public AudioClip GetAudioClipFromName(string name)
    {
        foreach (var soundEffect in soundeffects)
        {
            if (soundEffect.groupID == name)
            {
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
            }
        }
        Debug.LogWarning($"⚠️ No se encontró ningún grupo con el nombre: {name}");
        return null;
    }




   
}
