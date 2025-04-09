using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
    IEnumerator RespawnPoint()
    {
        Player.Instance.pState.cutscene = true;
    }
}
