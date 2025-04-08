using UnityEngine;

public class Crawler : Enemy
{
    [SerializeField] float flipWaitTime;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private LayerMask whatIsGround;
    float timer;
    
        
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        player = Player.Instance;
    }
    // Update is called once per frame
    
    protected override void UpdateEnemyState()
    {
        if (health <= 0)
        {
            Death(0.05f);
        }

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Crawler_Idle:
                Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;
                if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround) ||
                    Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    ChangeState(EnemyStates.Crawler_Flip);
                }
                
                if (transform.localScale.x>0)
                {
                    rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                }
                break;
            case EnemyStates.Crawler_Flip:
                timer += Time.deltaTime;
                if (timer >= flipWaitTime)
                {
                    timer = 0;
                   transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
                    ChangeState(EnemyStates.Crawler_Idle);
                }
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangeState(EnemyStates.Crawler_Flip);
        }
    }
}
