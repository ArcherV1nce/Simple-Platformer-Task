using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundControl : MonoBehaviour
{
    [SerializeField] protected float _audioVolume = 0.1f;

    protected const float MinVolume = 0f;
    
    protected AudioSource _audioSource;

    protected virtual void OnEnable()
    {
        SetUp();
    }

    protected virtual void OnValidate()
    {
        if (_audioVolume < MinVolume)
        {
            _audioVolume = MinVolume;
        }
    }

    protected virtual void PlaySound(AudioClip sound)
    {
        _audioSource.PlayOneShot(sound);
    }

    protected virtual void SetUp()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _audioVolume;
    }
}
