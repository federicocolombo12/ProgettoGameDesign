using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Player _player;
    void Start()
    {
        _player = Player.Instance;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player.playerHealth.TakeDamage(1);
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        
    }
}
