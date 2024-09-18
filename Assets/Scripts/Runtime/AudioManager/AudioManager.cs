using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip[] _audio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(int index)
    {
        if (_bgmSource.clip == _audio[index]) return;
        _bgmSource.clip = _audio[index];
        _bgmSource.loop = true;
        _bgmSource.Play();
    }

    public void PlaySFX(int index)
    {
        _sfxSource.PlayOneShot(_audio[index]);
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }
}