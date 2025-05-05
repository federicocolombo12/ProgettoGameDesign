using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public SfxEventChannel sfxChannel;
    public MusicEventChannel musicChannel;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        sfxChannel.OnSFXPlayRequested += PlaySFX;
        musicChannel.OnMusicChangeRequested += PlayMusic;
    }

    private void OnDisable()
    {
        sfxChannel.OnSFXPlayRequested -= PlaySFX;
        musicChannel.OnMusicChangeRequested -= PlayMusic;
    }

    public void PlaySFX(SfxData data)
    {
        sfxSource.pitch = data.pitch;
        sfxSource.PlayOneShot(data.clip, data.volume);
    }
    public void PlayMusic(AudioClip newClip, float fadeDuration)
    {
        StartCoroutine(FadeMusic(newClip, fadeDuration));
    }
    private IEnumerator FadeMusic(AudioClip newClip, float duration)
    {
        // fade out
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }

        musicSource.clip = newClip;
        musicSource.Play();

        // fade in
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
    }
}
