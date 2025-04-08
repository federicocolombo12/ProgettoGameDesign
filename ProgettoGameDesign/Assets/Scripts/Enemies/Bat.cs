using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;
    float timer;
    protected override void Start()
    {
        base.Start();
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
                rb.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime));
                FlipBat();
                break;
            case EnemyStates.Bat_Stunned:
                timer += Time.deltaTime;
                if (timer> stunDuration)
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
    }
}
