using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered respawn point");
            // Set the player's respawn point to this position
            GameManager.Instance.SetRespawnPoint(transform.position);
        }
    }
}
