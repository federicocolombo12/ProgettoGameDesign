using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{
    
    [Header("Health")]
    [SerializeField] int health;
    public int maxHealth = 100;
    [SerializeField] float invincibleTime = 1;
    [SerializeField] GameObject blood;
    [SerializeField] float hitFlashSpeed;
    float restoreTimeSpeed;
    bool restoreTime;
    public delegate void OnHealtChangedDelegate();
    [HideInInspector] public OnHealtChangedDelegate OnHealthChangedCallback;
    [Space(10)]
    [Header("Healing")]
    [SerializeField] float timeToHeal = 1;
    float healTimer;
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
    
    private void Start()
    {
        pState = GetComponent<PlayerStateList>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        Health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        Mana = mana;
        manaStorage.fillAmount = mana;
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
    public void RestoreTimeScale()
    {

        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {

                Time.timeScale += restoreTimeSpeed * Time.unscaledDeltaTime;
            }
            else
            {

                Time.timeScale = 1;
                restoreTime = false;

            }


        }

    }
    public void HitStopTime(float _newTimeScale, float _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;
        if (_delay > 0)
        {
            Debug.Log(_delay);
            StartCoroutine(StartTimeAgain());
        }
        else
        {

            restoreTime = true;
        }
    }
    IEnumerator StartTimeAgain()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        restoreTime = true;





    }
    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Math.Clamp(value, 0, maxHealth);
                if (OnHealthChangedCallback != null)
                {

                    OnHealthChangedCallback.Invoke();
                }
            }
        }
    }
    
    public void Heal(bool healPressed)
    {
        if (healPressed && Health < maxHealth && playerMovement.IsGrounded() && !pState.dashing && Mana > 0)
        {
            animator.SetBool("Healing", true);
            pState.healing = true;
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
            pState.healing = false;
            healTimer = 0;
            animator.SetBool("Healing", false);
        }
    }
    public void TakeDamage(float damage)
    {
        Health -= Mathf.RoundToInt(damage);
        StartCoroutine(StopTakingDamage());

    }
    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;

        animator.SetTrigger("TakeDamage");
        GameObject schizzoSangue = Instantiate(blood, transform);
        Destroy(schizzoSangue, 1.5f);
        yield return new WaitForSeconds(invincibleTime);
        pState.invincible = false;
    }
    public void FlashWhileInvincible()
    {
        sr.material.color = pState.invincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : Color.white;
    }
}
