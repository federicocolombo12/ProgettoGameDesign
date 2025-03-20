using UnityEngine;

public class Zombie : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.gravityScale = 30f;
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
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(player.transform.position.x,
                0),
                speed * Time.deltaTime);
                
        }
    }
    public override void EnemyHit(float damage, Vector2 hitDirection, float _hitForce)
    {
        base.EnemyHit(damage, hitDirection, _hitForce);
    }
}
