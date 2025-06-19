using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public SfxEventChannel sfxChannel;
    public MusicEventChannel musicChannel;
    [SerializeField] float audioCooldown = 0.1f;

    private HashSet<AudioClip> cooldownClips = new HashSet<AudioClip>();

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

    public void PlaySFX(SfxData data, bool randomizePitch)
{
    if (cooldownClips.Contains(data.clip))
        return;

    sfxSource.pitch = data.pitch;
    if (randomizePitch)
    {
        sfxSource.pitch = Random.Range(data.pitch - 0.1f, data.pitch + 0.1f);
    }
    sfxSource.PlayOneShot(data.clip, data.volume);
    Debug.Log("Playing SFX: " + data.clip.name);

       

    // Start cooldown coroutine
        StartCoroutine(HandleCooldown(data.clip, audioCooldown));
}
    public void PlaySFXContinuos(SfxData data, bool randomizePitch)
    {
        

        sfxSource.pitch = data.pitch;
        if (randomizePitch)
        {
            sfxSource.pitch = Random.Range(data.pitch - 0.1f, data.pitch + 0.1f);
        }
        sfxSource.PlayOneShot(data.clip, data.volume);
        Debug.Log("Playing SFX: " + data.clip.name);



        // Start cooldown coroutine
        StartCoroutine(HandleCooldown(data.clip, audioCooldown));
    }

    private IEnumerator HandleCooldown(AudioClip clip, float cooldown)
{
    cooldownClips.Add(clip);
    yield return new WaitForSeconds(cooldown);
    cooldownClips.Remove(clip);
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
        //loop it
        musicSource.loop = true;
        musicSource.Play();

        // fade in
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
    }
    public void DisableVolume()
    {
            sfxSource.volume = 0f;
        musicSource.volume = 0f;
    }
    public void EnableVolume()
    {
        sfxSource.volume = 1f;
        musicSource.volume = 1f;
    }
}
