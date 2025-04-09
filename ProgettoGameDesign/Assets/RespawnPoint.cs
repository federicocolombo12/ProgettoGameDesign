using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the player's respawn point to this position
            GameManager.Instance.platformRespawnPoint = transform.position;
        }
    }
}
