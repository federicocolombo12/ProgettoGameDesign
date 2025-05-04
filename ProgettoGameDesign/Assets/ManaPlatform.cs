using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ManaPlatform : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
    public override void EnemyHit(float damage, Vector2 hitDirection, float _hitForce)
    {
        health -= damage;
        sr.color = new Color(1, 1, 1, health/10);
        if (health <= 0)
        {
            Death(0.05f);
        }

    }
    protected override void OnCollisionStay2D(Collision2D collision)
    {
       
    }

}
