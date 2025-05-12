using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    public float health;
    [SerializeField] protected float recoilLenght;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] private float restoreTimeSpeed;
    [SerializeField] protected Player player;
    [SerializeField] protected float speed;
    [SerializeField] protected LayerMask playerLayer;
    protected float recoilTimer;
    [SerializeField] protected float damage;
    [SerializeField] GameObject orangeBlood;
    public static Action OnEnemyDeath;
    protected Animator animator;
    protected Tween hurtTween;


    protected enum EnemyStates
    {
        // Crawler states
        Crawler_Idle,
        Crawler_Flip,
        // Bat states
        Bat_Idle,
        Bat_Chase,
        Bat_Stunned,
        Bat_Death,
        // Charger states
        Charger_Idle,
        Charger_Surprised,
        Charger_Charge,


    }
    protected EnemyStates currentEnemyState;
    protected virtual EnemyStates GetCurrentEnemyState
    {
        get { return currentEnemyState; }
        set { if (currentEnemyState != value)
            {
                currentEnemyState = value;
                ChangeCurrentAnimation();
            }; }
    }
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = Player.Instance;
        
    }
    
    protected virtual void Update()
    {
        
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
        else
        {
            UpdateEnemyState();
        }
    
    }
    public virtual void EnemyHit(float damage, Vector2 hitDirection, float _hitForce)
    {
        health -= damage;
        if (!isRecoiling)
        {
            GameObject _orangeBlood = Instantiate(orangeBlood, transform.position, Quaternion.identity);
            Destroy(_orangeBlood, 5.5f);
            rb.linearVelocity= hitDirection * _hitForce*recoilFactor;
            isRecoiling = true;
            EnemyHitFeedback();
        }
    }
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player")&&!player.pState.invincible
            &&health>0)
        {
            Attack();
            player.playerHealth.HitStopTime(0.1f,restoreTimeSpeed,0.5f);
        }


    }
    protected virtual void Death(float _destroyTime) 
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject, _destroyTime);
    }
    protected virtual void UpdateEnemyState()
    {
        
    }
    protected void ChangeState(EnemyStates _newState)
    {
        GetCurrentEnemyState = _newState;
    }
    protected virtual void ChangeCurrentAnimation()
    {
        
        
       
    }

    protected virtual void EnemyHitFeedback()
    {
        if (hurtTween != null)
        {
            hurtTween.Kill();
        }

        // Flash white
        Debug.Log("Flashing white");
        Color originalColor = sr.color;


        hurtTween = sr.DOColor(Color.white, 0.5f).OnComplete(() =>
        {
            // Return to original color
            sr.DOColor(originalColor, 0.1f);
        });
        
    }



    protected virtual void Attack()
    {
        player.playerHealth.TakeDamage(damage);
    }

}
