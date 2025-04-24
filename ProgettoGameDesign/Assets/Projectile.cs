using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Player _player;
    void Start()
    {
        _player = Player.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _player.playerHealth.TakeDamage(1);
    }
}
