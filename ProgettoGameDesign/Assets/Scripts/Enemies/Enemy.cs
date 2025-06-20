using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    public SpriteRenderer sr;
    
    protected Material standardMaterial;
    public float health;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float recoilLenght;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] private float restoreTimeSpeed;
    [SerializeField] protected Player player;
    [SerializeField] protected float speed;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Material flashMaterial;
    [SerializeField] protected ParticleSystem deathEffect;
    public Image healthImage;
    protected float recoilTimer;
    [SerializeField] protected float damage;
    protected float currentDamage;
    [SerializeField] GameObject orangeBlood;
    public PlayerTransformation weakTo;
    
    [SerializeField] PlayerTransformation immuneTo;
    [SerializeField] ParticleSystem weakToEffect;
    [SerializeField] protected float damageMultiplier;
    

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
        if (GetComponent<SpriteRenderer>()==null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
       
        animator = GetComponent<Animator>();
        
        if (sr != null)
        {
            standardMaterial = sr.material;
        }
        
        player = Player.Instance;
        
        health = maxHealth;
        
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
        Debug.Log("Player Transformation: " + Player.Instance.playerTransformation);
        Debug.Log("Enemy Weakness: " + weakTo);
        EnemyWeakness();
        
        

        if (healthImage!= null){
            healthImage.fillAmount = health / maxHealth;
        }
       
        if (!isRecoiling)
        {
            EffectManager.Instance.PlayOneShot(orangeBlood.GetComponent<ParticleSystem>(), transform.position);
            if (recoilFactor == 0)
            {
                return;
            }
            rb.linearVelocity= hitDirection * _hitForce*recoilFactor;
            isRecoiling = true;
            
        }
    }
    private void EnemyWeakness()
    {
        currentDamage = damage;
        if (sr!= null)
        {
            sr.material = flashMaterial;
        }


        if (weakTo == Player.Instance.playerTransformation)
        {
            currentDamage *= damageMultiplier;
            if (weakToEffect != null)
            {
                // Play the weak to effect if it exists
                 EffectManager.Instance.PlayOneShot(weakToEffect, transform.position);
            }
            else
            {
                Debug.LogWarning("Weak to effect is not assigned for " + gameObject.name);
            }
           
        }
        if (immuneTo == Player.Instance.playerTransformation)
        {
            currentDamage *= 1 / damageMultiplier;
        }
        health -= currentDamage;

        // After 0.2 seconds restore the original material
        DOVirtual.DelayedCall(0.2f, () => {
            sr.material = standardMaterial;
        });
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player")&&!player.pState.invincible
            &&health>0)
        {
            Attack();
           
            
        }


    }
    protected virtual void Death(float _destroyTime) 
    {
        OnEnemyDeath?.Invoke();
        EffectManager.Instance.PlayOneShot(deathEffect, transform.position);
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

    



    protected virtual void Attack()
    {
        player.playerHealth.TakeDamage(damage);
    }

}
