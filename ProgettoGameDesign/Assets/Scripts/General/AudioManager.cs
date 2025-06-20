using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public SfxEventChannel sfxChannel;
    public MusicEventChannel musicChannel;
    [SerializeField] float audioCooldown = 0.1f;
    [SerializeField] AudioMixer mixer;
    [SerializeField] SfxData buttonClickSfx;

    private HashSet<AudioClip> cooldownClips = new HashSet<AudioClip>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        sfxChannel.OnSFXPlayRequested += PlaySFX;
        musicChannel.OnMusicChangeRequested += PlayMusic;
        SceneController.OnSceneLoaded += ResetVolume; // Disable volume when a new scene is loaded
    }

    private void ResetVolume()
    {
        sfxSource.clip = null;
        musicSource.clip = null;
    }
    public void PlayButtonSfx()
    {
        PlaySFX(buttonClickSfx, true);
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
    public void DisableSfx()
    {
           mixer.SetFloat("Sfx", -80f); // Mute SFX

    }
    public void DisableMusic()
    {
        mixer.SetFloat("Music", -80f); // Mute Music
    }
    public void EnableSfx()
    {
        mixer.SetFloat("Sfx", 0f); // Unmute SFX
    }
    public void EnableMusic()
    {
        mixer.SetFloat("Music", 0f); // Unmute Music
    }
    public void DisableEverything()
    {

        DisableSfx();
        DisableMusic();
    }
    
}
