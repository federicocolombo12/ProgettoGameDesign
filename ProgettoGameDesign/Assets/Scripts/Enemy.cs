using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLenght;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected PlayerController player;
    [SerializeField] protected float speed;
    protected float recoilTimer;
    [SerializeField] protected float damage;
    [SerializeField] protected int restoreTimeSpeed;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.Instance;
    }
    
    protected virtual void Update()
    {
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
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")&&!player.pState.invincible)
        {
            Attack();
            player.HitStopTime(0,restoreTimeSpeed,0.5f);
        }


    }
    protected virtual void Attack()
    {
        player.TakeDamage(damage);
    }

}
