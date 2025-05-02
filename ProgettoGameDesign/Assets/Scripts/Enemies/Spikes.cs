using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float respawnTime = 1f; // Time to wait before respawning the player

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Spike Triggered");
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(RespawnPoint());
        }
    }
    IEnumerator RespawnPoint()
    {
        Player.Instance.pState.cutscene = true;
        Player.Instance.pState.invincible = true;
        Player.Instance.rb.linearVelocity = Vector2.zero;
        Time.timeScale = 0f;
        StartCoroutine(UiManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        Player.Instance.playerHealth.TakeDamage(1);
        yield return new WaitForSecondsRealtime(respawnTime);
        
        Player.Instance.transform.position = GameManager.Instance.platformRespawnPoint;
        StartCoroutine(UiManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        yield return new WaitForSecondsRealtime(UiManager.Instance.sceneFader.fadeTime);
        Player.Instance.pState.cutscene = false;
        Player.Instance.pState.invincible = false;
        Time.timeScale = 1f;
    }
}
