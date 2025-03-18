using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] float speed = 5f;
    private Vector2 moveInput;
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
    bool attack=false;
    [SerializeField] float timeBetweenAttack, timeSinceAttack;




    public static PlayerController Instance { get; private set; }
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerStateList pState;
    private float gravityScale;


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
        animator = GetComponent<Animator>();
        pState = GetComponent<PlayerStateList>();
        gravityScale = rb.gravityScale;
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
        moveInput = input;
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
    void ResetDash() { 
        
        if (IsGrounded())
        {
            dashed = false;
        }
    }

    private void FixedUpdate()
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
    }

    void Flip()
    {
        if (moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    void Move()
    {
        // Preserva la velocit� verticale (gestita dalla gravit� o da altri effetti fisici)
        Vector2 newVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;
        animator.SetBool("Walking", rb.linearVelocity.x !=0 &&IsGrounded());
    }
    public bool IsGrounded()
    {

        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckY, groundLayer)||
            Physics2D.Raycast(groundCheck.position+ new Vector3(GroundCheckX,0,0), Vector2.down, groundCheckY, groundLayer)
            ||Physics2D.Raycast(groundCheck.position+   new Vector3(-GroundCheckX,0,0), Vector2.down, groundCheckY, groundLayer);
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
            jumpBufferCounter --;
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
            //Attacco
        }
    }
}
