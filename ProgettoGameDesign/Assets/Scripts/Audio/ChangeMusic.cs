using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] AudioClip newMusicClip;
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlayMusic(newMusicClip, 1f);
    }

    // Update is called once per frame

}
