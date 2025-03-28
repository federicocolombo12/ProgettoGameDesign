using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] float speed = 5f;
    private Vector2 directionalInput;
    [Space(10)]
    
    [Header("Jumping Mechanics")]
    
    [SerializeField] float jumpForce = 10f;
    private float jumpInput;
    float jumpBufferCounter = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int jumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckY = 0.2f;
    [SerializeField] float GroundCheckX = 0.5f;
    [SerializeField] LayerMask groundLayer;
    [Space(10)]

    [Header("Dashing")]
    
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] GameObject dashEffect;
    private bool canDash = true;
    private bool dashed = false;

    [Space(10)]

    [Header("Attack")]
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] Transform sideAttackTransform, upAttackTransform, downAttackTransform;
    [SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] float damage = 10;
    [SerializeField] float hitForce = 10;
    [SerializeField] GameObject slashEffect;
    bool attack = false;
    [SerializeField] float timeBetweenAttack, timeSinceAttack;
    [Space(10)]
    [Header("Recoil")]
    [SerializeField] float recoilXSpeed = 1f;
    [SerializeField] float recoilYSpeed = 1f;
    [SerializeField] int recoilXSteps = 10;
    [SerializeField] int recoilYSteps = 10;
    int stepsXRecoiled = 0;
    int stepsYRecoiled = 0;
    [Space(10)]
    [Header("Health")]
    [SerializeField] int health ;
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
    bool healPressed = false;
    [Space(10)]
    [Header("Mana")]
    [SerializeField] float mana;
    [SerializeField] float manaDrainSpeed;
    [SerializeField] float manaGain;



    public static PlayerController Instance { get; private set; }
    private Rigidbody2D rb;
    private Animator animator;
    public PlayerStateList pState;
    private float gravityScale;
    private SpriteRenderer sr;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InputManager.Instance.OnMoveInput += HandleMoveInput;
        InputManager.Instance.OnJumpInput += HandleJump;
        InputManager.Instance.OnDashInput += HandleDash;
        InputManager.Instance.OnAttackInput += HandleAttack;
        InputManager.Instance.OnHealInput += HandleHeal;
        animator = GetComponent<Animator>();
        pState = GetComponent<PlayerStateList>();
        gravityScale = rb.gravityScale;
        Health= maxHealth;
        sr = GetComponent<SpriteRenderer>();
        Mana = mana;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
    }

   


    private void OnDisable()
    {
        // Rimozione delle sottoscrizioni per evitare errori
        InputManager.Instance.OnMoveInput -= HandleMoveInput;
        InputManager.Instance.OnJumpInput -= HandleJump;

        InputManager.Instance.OnDashInput -= HandleDash;
        InputManager.Instance.OnAttackInput -= HandleAttack;
    }

    private void HandleMoveInput(Vector2 input)
    {
        directionalInput = input;
    }

    private void HandleJump(float input)
    {
        // Logica di salto, ad esempio:
        Debug.Log("Salto eseguito!");
        jumpInput = input;
        // Puoi implementare qui l'effettivo salto del giocatore
    }
    private void HandleDash() {
        if (canDash && !dashed)
        {
            dashed = true;
            StartCoroutine(Dash());
        }
        
        
    }
    private void HandleAttack()
    {
        attack = true;
    }
    private void HandleHeal(bool healValue)
    {
        healPressed=healValue;
    }
    void ResetDash() { 
        
        if (IsGrounded())
        {
            dashed = false;
        }
    }

    void Update()
    {
        UpdateJumpVariables();
        if (pState.dashing)
        {
            return;
        }
        Move();
        Jump();
        ResetDash();
        Flip();
        Attack();
        
        RestoreTimeScale();
        FlashWhileInvincible();
        Heal();
    }
    private void FixedUpdate()
    {
        Recoil();
    }

    void Flip()
    {
        if (directionalInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            pState.lookingRight = true;
        }
        else if (directionalInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            pState.lookingRight = false;
        }
    }
    void Move()
    {
        // Preserva la velocit� verticale (gestita dalla gravit� o da altri effetti fisici)
        Vector2 newVelocity = new Vector2(directionalInput.x * speed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;
        animator.SetBool("Walking", rb.linearVelocity.x !=0 &&IsGrounded());
    }
    public bool IsGrounded()
    {

        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckY, groundLayer)||
            Physics2D.Raycast(groundCheck.position+ new Vector3(GroundCheckX,0,0), Vector2.down, groundCheckY, groundLayer)
            ||Physics2D.Raycast(groundCheck.position+   new Vector3(-GroundCheckX,0,0), Vector2.down, groundCheckY, groundLayer);
    }
    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }
    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }
    void Jump()
    {
        if (jumpInput == 0 && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            pState.jumping = false;
        }

        if (!pState.jumping)
        {
            if (coyoteTimeCounter>0&&jumpBufferCounter>0) 
            { 
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpInput * jumpForce);
                pState.jumping = true;
            }
            else if (!IsGrounded() && jumpCount < maxJumpCount && jumpInput!=0)
            {
                pState.jumping = true;
                jumpCount++;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpInput * jumpForce);
                
                
            }
            
        }
        animator.SetBool("Jumping", !IsGrounded());
        

    }
    void UpdateJumpVariables()
    {
        if (IsGrounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            jumpCount = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (jumpInput!=0)
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter -=Time.deltaTime*10;
        }
    }
    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        animator.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x*dashSpeed, 0);
        if (IsGrounded()) { Instantiate(dashEffect, transform); }
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravityScale;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {

            timeSinceAttack = 0;
            animator.SetTrigger("Attack");
            attack = false;
            Debug.Log("Attacco eseguito!");

            if (Mathf.Abs(directionalInput.y) < 0.3f || (directionalInput.y < 0 && IsGrounded()))
            {
                Debug.Log("Attacco laterale");
                Hit(sideAttackTransform, sideAttackArea, ref pState.recoilingX, recoilXSpeed);
                Instantiate(slashEffect, sideAttackTransform);
            }
            else if (directionalInput.y > 0.3f)
            {
                Debug.Log("Attacco alto");
                Hit(upAttackTransform, upAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAngle(slashEffect, 90, upAttackTransform);
            }
            else if (directionalInput.y < 0.3f && !IsGrounded())
            {
                Debug.Log("Attacco basso");
                Hit(downAttackTransform, downAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAngle(slashEffect, -90, downAttackTransform);
            }
            // Attacco
        }
    }

    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoildDir, float _recoilStrenght)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        if (hits.Length > 0)
        {
            _recoildDir = true;
            for (int i = 0; i < hits.Length; i++)
            {
                

                hits[i].GetComponent<Enemy>().EnemyHit(
                    damage, transform.position - 
                    hits[i].transform.position, _recoilStrenght);
                if (hits[i].CompareTag("Enemy"))
                {
                    Mana += manaGain;
                }
            }
        }
    }
    void SlashEffectAngle(GameObject slashEffect, int angle, Transform attackTransform)
    {
        GameObject slash = Instantiate(slashEffect, attackTransform);
        slash.transform.eulerAngles = new Vector3(0, 0, angle);
        slash.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        
    }
    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(recoilXSpeed, 0);
            }

        }
        if (pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (directionalInput.y < 0)
            {
                
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -recoilYSpeed);
                
            }
            jumpCount = 0;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
        if (pState.recoilingX&&stepsXRecoiled<recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }
        if (IsGrounded())
        {
            StopRecoilY();
        }
    }
    void RestoreTimeScale()
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
    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
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

        restoreTime = true;

        yield return new WaitForSeconds(0.5f);
        
        
        
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
    float Mana
    {
        get { return mana; }
        set
        {
            if (mana != value)
            {
                mana = Mathf.Clamp(value, 0, 1);
                
            }
            
        }
    }
    void Heal()
    {
        if (healPressed && Health < maxHealth && !pState.jumping && !pState.dashing && Mana>0)
        {
            animator.SetBool("Healing", true);
            pState.healing = true;
            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                Health++;
                healTimer = 0;
            }
            Mana-= Time.deltaTime * manaDrainSpeed;
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
        GameObject schizzoSangue=Instantiate(blood, transform);
        Destroy(schizzoSangue, 1.5f);
        yield return new WaitForSeconds(invincibleTime);
        pState.invincible = false;
    }
    void FlashWhileInvincible()
    {
        sr.material.color=pState.invincible?Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time*hitFlashSpeed, 1.0f)):Color.white;
    }
}
