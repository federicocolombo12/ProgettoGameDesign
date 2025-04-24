using UnityEngine;

public class Charger : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float chargeDuration;

    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    float timer;


    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Charger_Idle);
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
        Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;
        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Charger_Idle:
                
                if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround) ||
                    Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);

                    
                }
                RaycastHit2D _hit = Physics2D.Raycast(transform.position + _ledgeCheckStart, _wallCheckDir, ledgeCheckX * 10);
                if (_hit.collider != null && _hit.collider.gameObject.CompareTag("Player"))
                {
                    ChangeState(EnemyStates.Charger_Surprised);
                }

                if (transform.localScale.x > 0)
                {
                    rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                }
                break;
            case EnemyStates.Charger_Surprised:
                rb.linearVelocity = new Vector2(0, jumpForce);
                ChangeState(EnemyStates.Charger_Charge);
                break;
            case EnemyStates.Charger_Charge:
                timer += Time.deltaTime;
                if (timer < chargeDuration)
                {
                    if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround) ||
                    Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);


                    }
                    if (Physics2D.Raycast(transform.position, Vector2.down, ledgeCheckY, whatIsGround))
                    {

                        if (transform.localScale.x > 0)
                        {
                            rb.linearVelocity = new Vector2(speed * chargeSpeedMultiplier, rb.linearVelocity.y);
                        }
                        else
                        {
                            rb.linearVelocity = new Vector2(-speed * chargeSpeedMultiplier, rb.linearVelocity.y);
                        }
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    }
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Charger_Idle);
                }
                break;
        }
    }
    protected override void ChangeCurrentAnimation()
    {
        if (GetCurrentEnemyState == EnemyStates.Charger_Idle)
        {
            animator.speed = 1;
        }
        
        if (GetCurrentEnemyState == EnemyStates.Charger_Charge)
        {
            animator.speed = chargeSpeedMultiplier;
        }
    }
}
