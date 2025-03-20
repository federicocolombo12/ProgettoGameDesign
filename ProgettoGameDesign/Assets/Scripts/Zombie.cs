using UnityEngine;

public class Zombie : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.gravityScale = 12f;
        player = PlayerController.Instance;
    }
    protected override void Awake()
    {
        base.Awake();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isRecoiling)
        {
            Vector2 direction = (new Vector2(player.transform.position.x - transform.position.x, 0)).normalized;
            transform.Translate(direction*speed*Time.deltaTime);
                
        }
    }
    public override void EnemyHit(float damage, Vector2 hitDirection, float _hitForce)
    {
        base.EnemyHit(damage, hitDirection, _hitForce);
    }
}
