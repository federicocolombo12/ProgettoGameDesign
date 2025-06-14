using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.Utilities;

public class Bat : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;
    [SerializeField] SpriteRenderer batSr;
    [SerializeField] private LayerMask obstacleMask;
    private Vector3 initialPosition;
    [SerializeField] private float idleReturnTime = 3f;
    private bool isReturningToStart = false;


    private Tween moveTween;
    float timer;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = batSr;
        animator = GetComponentInChildren<Animator>();
        initialPosition = transform.position;
        standardMaterial = sr.material;
        player = Player.Instance;
        
        health = maxHealth;
        ChangeState(EnemyStates.Bat_Idle);
    }

    // Update is called once per frame
    protected override void UpdateEnemyState()
    {
        float _dist = Vector2.Distance(transform.position, player.transform.position);
        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Bat_Idle:
                if (_dist < chaseDistance && HasLineOfSightToPlayer())
                {
                    timer = 0;
                    isReturningToStart = false;
                    ChangeState(EnemyStates.Bat_Chase);
                }
                else
                {
                    timer += Time.deltaTime;

                    if (timer >= idleReturnTime && !isReturningToStart && Vector2.Distance(transform.position, initialPosition) > 0.1f)
                    {
                        isReturningToStart = true;
                        Vector2 _direction = (initialPosition - transform.position).normalized;
                        rb.linearVelocity = new Vector2(_direction.x * speed, _direction.y * speed);
                    }
                }
                break;


            case EnemyStates.Bat_Chase:
                Vector2 direction = (player.transform.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(direction.x * speed, direction.y * speed);


                FlipBat();
                if (_dist > chaseDistance)
                {
                    moveTween?.Kill(); // Stop movement tween
                    ChangeState(EnemyStates.Bat_Idle);
                }
                break;
            case EnemyStates.Bat_Stunned:
                timer += Time.deltaTime;
                if (timer > stunDuration)
                {
                    timer = 0;
                    ChangeState(EnemyStates.Bat_Idle);
                }
                break;
            case EnemyStates.Bat_Death:
                Death(0f);
                break;
        }
    }

    void FlipBat()
    {
        sr.flipX = player.transform.position.x < transform.position.x;
    }

    public override void EnemyHit(float damage, Vector2 hitDirection, float _hitForce)
    {
        base.EnemyHit(damage, hitDirection, _hitForce);
        if (health <= 0)
        {
            moveTween?.Kill(); // Stop movement tween if the bat is dead
            ChangeState(EnemyStates.Bat_Death);
        }
        else
        {
            ChangeState(EnemyStates.Bat_Stunned);
        }
    }

    protected override void ChangeCurrentAnimation()
    {
        animator.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Bat_Idle);
        animator.SetBool("Chasing", GetCurrentEnemyState == EnemyStates.Bat_Chase);
        animator.SetBool("Stunned", GetCurrentEnemyState == EnemyStates.Bat_Stunned);
        if (GetCurrentEnemyState == EnemyStates.Bat_Death)
        {
            animator.SetTrigger("Death");
        }
    }

    protected override void Death(float _destroyTime)
    {
        base.Death(_destroyTime);
        rb.gravityScale = 2f;
        gameObject.layer = LayerMask.NameToLayer("Background");
        DOVirtual.DelayedCall(_destroyTime, () =>
        {
            Destroy(gameObject);
        });

    }
    private bool HasLineOfSightToPlayer()
    {
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);

        // Se il raycast NON colpisce niente, allora c'è linea visiva libera
        return hit.collider == null;
    }

}
