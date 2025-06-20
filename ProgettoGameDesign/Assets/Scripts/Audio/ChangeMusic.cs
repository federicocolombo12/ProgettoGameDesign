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
        if (collision.CompareTag("Player") && newMusicClip != null) {
            AudioManager.Instance.PlayMusic(newMusicClip, 1f);
        }
        Destroy(gameObject, 0.1f); // Destroy the trigger after it has been used
        
    }

    // Update is called once per frame

}
