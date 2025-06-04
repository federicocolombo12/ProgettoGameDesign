using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Player _player;
    public GameObject shooter;
    Rigidbody2D rb;
    float projectileDamage = 1f;

    void Start()
    {
        _player = Player.Instance;
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player.playerHealth.TakeDamage(projectileDamage);
        }
        if (!other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        
    }
    [Button("Set Force")][SerializeField]
    public void SetForce(Vector2 force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
