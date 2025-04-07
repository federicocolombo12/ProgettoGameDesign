using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLenght;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected Player player;
    [SerializeField] protected float speed;
    protected float recoilTimer;
    [SerializeField] protected float damage;
    [SerializeField] protected float restoreTimeSpeed;

    protected enum EnemyStates
    {
        Crawler_Idle,
        Crawler_Flip
    }
    protected EnemyStates currentEnemyState;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = Player.Instance;
    }
    
    protected virtual void Update()
    {
        UpdateEnemyState();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (isRecoiling ) {
            
            if (recoilTimer < recoilLenght)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    
    }
    public virtual void EnemyHit(float damage, Vector2 hitDirection, float _hitForce)
    {
        health -= damage;
        if (!isRecoiling)
        {
            rb.AddForce(-hitDirection * _hitForce*recoilFactor);
            isRecoiling = true;
        }
    }
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player")&&!player.pState.invincible)
        {
            Attack();
            player.playerHealth.HitStopTime(0.1f,restoreTimeSpeed,0.5f);
        }


    }
    protected virtual void UpdateEnemyState()
    {
        
    }
    protected void ChangeState(EnemyStates _newState)
    {
        currentEnemyState = _newState;
    }
    
    
    protected virtual void Attack()
    {
        player.playerHealth.TakeDamage(damage);
    }

}
