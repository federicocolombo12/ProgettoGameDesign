using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    public int maxHealth = 100;
    [SerializeField] float invincibleTime = 1;
    [SerializeField] GameObject hitEffect1;
    [SerializeField] GameObject blood;
    [SerializeField] float hitFlashSpeed;
    [SerializeField] float hitStopTimeScale;
   [SerializeField]  float hitStopTimeDuration;

    public delegate void OnHealtChangedDelegate();
    [HideInInspector] public OnHealtChangedDelegate OnHealthChangedCallback;

    [Space(10)]
    [Header("Healing")]
    [SerializeField] float timeToHeal = 1;
    float healTimer;
    float damageMultiplier = 1;
    [SerializeField] ParticleSystem healEffect;
    [Space(10)]
    [Header("Mana")]
    [SerializeField] Image manaStorage;
    [SerializeField] float mana;
    [SerializeField] float manaDrainSpeed;
    public float manaGain;

    private Animator animator;
    private SpriteRenderer sr;
    private PlayerMovement playerMovement;
    public PlayerStateList pState { get; private set; }

    public static event Action OnPlayerDeath;
    public static event Action<float, Color> OnPlayerHit;

    void OnEnable()
    {
        PlayerTransform.OnTransform += SetDamageMultiplier;
        PlayerTransform.OnTransform += UpdateVariables;
    }

    void OnDisable()
    {
        PlayerTransform.OnTransform -= SetDamageMultiplier;
        PlayerTransform.OnTransform -= UpdateVariables;
    }

    private void Start()
    {
        pState = Player.Instance.pState;
        playerMovement = Player.Instance.playerMovement;
        animator = Player.Instance.animator;
        Health = maxHealth;
        sr = Player.Instance.sr;
        Mana = mana;
        manaStorage.fillAmount = mana;
        pState.alive = true;
    }

    void UpdateVariables()
    {
         animator = Player.Instance.animator;
    }

    void SetDamageMultiplier()
    {
        damageMultiplier = Player.Instance.playerTransformation.damageMultiplier;
        OnHealthChangedCallback?.Invoke();
    }

    public float Mana
    {
        get { return mana; }
        set
        {
            if (mana != value)
            {
                mana = Mathf.Clamp(value, 0, 1);
                manaStorage.fillAmount = mana;
            }
        }
    }

    

    

    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Math.Clamp(value, 0, maxHealth);
                OnHealthChangedCallback?.Invoke();
            }
        }
    }

   

    public void Heal(bool healPressed)
    {
        if (healPressed && Health < maxHealth && playerMovement.IsGrounded() && !pState.dashing && Mana > 0)
        {
            animator.SetBool("Healing", true);
            pState.healing = true;
            if (!healEffect.isPlaying)
            {
                healEffect?.Play();
            }
            
            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                Health++;
                healTimer = 0;
            }
            Mana -= Time.deltaTime * manaDrainSpeed;
        }
        else
        {
            if (healEffect.isPlaying)
            {
                healEffect?.Stop();
                healEffect?.Clear();
            }
            pState.healing = false;
            healTimer = 0;
            animator.SetBool("Healing", false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (pState.alive && !pState.invincible)
        {
            Health -= Mathf.RoundToInt(damage * damageMultiplier);
            CameraManager.Instance.ShakeCamera(0.1f);
            EffectManager.Instance.TimeStopEffect(hitStopTimeScale, hitStopTimeDuration);
            if (Health <= 0 && pState.alive)
            {
                Health = 0;
                StartCoroutine(Death());
            }
            else
            {
                StartCoroutine(StopTakingDamage());
            }
        }
    }

    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        animator.SetTrigger("TakeDamage");
        EffectManager.Instance.PlayOneShot(blood.GetComponent<ParticleSystem>(), transform.position);
        EffectManager.Instance.PlayOneShot(hitEffect1.GetComponent<ParticleSystem>(), transform.position);
        //disable collision
        Player.Instance.rb.excludeLayers = LayerMask.GetMask("Attackable");
        if (!healEffect.isPlaying)
        {

            healEffect?.Play();
        }
        
        OnPlayerHit?.Invoke(invincibleTime, Color.red);
        yield return new WaitForSeconds(invincibleTime);
        //enable collision
        Player.Instance.rb.excludeLayers = LayerMask.GetMask("Nothing");
        healEffect.Stop();
        pState.invincible = false;
        Player.Instance.coll.enabled = true;
    }

    public void FlashWhileInvincible()
    {
        //sr.material.color = pState.invincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : Color.white;
    }

    IEnumerator Death()
    {
        OnPlayerDeath?.Invoke();
        pState.alive = false;
        Time.timeScale = 0.0f;
        GameObject schizzoSangue = Instantiate(blood, transform);
        Destroy(schizzoSangue, 1.5f);
        animator.SetTrigger("Death");
        StartCoroutine(SceneFader.Instance.Fade(SceneFader.FadeDirection.In));
        yield return new WaitForSecondsRealtime(3f);
        Mana = 0;
        Health = maxHealth;
        GameManager.Instance.RespawnPlayer(Player.Instance);
        Time.timeScale = 1.0f;
    }

    public void Respawned()
    {
        if (!pState.alive)
        {
            pState.alive = true;
            Health = maxHealth;
            animator.Play("Idle");
        }
    }

}
