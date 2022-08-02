using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicControl : SoundControl
{
    protected void Awake()
    {
        DontDestroy();
    }

    private void DontDestroy()
    {
        MusicControl[] objects = FindObjectsOfType<MusicControl>();

        if (objects.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
