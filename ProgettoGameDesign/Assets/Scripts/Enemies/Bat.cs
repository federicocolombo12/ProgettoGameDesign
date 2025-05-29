using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Bat : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;
    float timer;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren <Animator>();

        standardMaterial = sr.material;
        player = Player.Instance;
        healthImage = GetComponentInChildren<Image>();
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
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Bat_Chase);
                }
                break;
            case EnemyStates.Bat_Chase:
                transform.DOMove(player.transform.position, 1f / speed).SetEase(Ease.Linear);
                FlipBat();
                if (_dist > chaseDistance)
                {
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
                Death(Random.Range(5, 10));
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
        rb.gravityScale = 12f;
        gameObject.layer = LayerMask.NameToLayer("Background");
    }
}
