using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput pInput;
    [Header("Horizontal Movement")]
    [SerializeField] float speed = 5f;
   
    [Space(10)]

    [Header("Jumping Mechanics")]

    [SerializeField] float jumpForce = 10f;
    
    float jumpBufferCounter = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    public int jumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] float jumpGScale = 2f;
    [SerializeField] float fallGScale = 0.5f;
    [Space(10)]
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckY = 0.2f;
    [SerializeField] float GroundCheckX = 0.5f;
    [SerializeField] LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerStateList pState;
    public float gravityScale { get; private set; }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pState = GetComponent<PlayerStateList>();
        gravityScale = rb.gravityScale;
    }
    private void Update()
    {
        UpdateVariables();
    }

    // Update is called once per frame
    void UpdateVariables()
    {
        speed = Player.Instance.playerTransformation.moveSpeed;
        jumpForce = Player.Instance.playerTransformation.jumpForce;
        maxJumpCount = Player.Instance.playerTransformation.jumpCount;
    }
    void Flip(Vector2 directionalInput)
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
    public void Move(Vector2 directionalInput)
    {
        // Preserva la velocit� verticale (gestita dalla gravit� o da altri effetti fisici)
        if (pState.healing)
        {
            rb.linearVelocity = new Vector2(0, 0);
            return;
        }
        Vector2 newVelocity = new Vector2(directionalInput.x * speed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;
        animator.SetBool("Walking", rb.linearVelocity.x != 0 && IsGrounded());
        Flip(directionalInput);
    }
    public bool IsGrounded()
    {

        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckY, groundLayer) ||
            Physics2D.Raycast(groundCheck.position + new Vector3(GroundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer)
            || Physics2D.Raycast(groundCheck.position + new Vector3(-GroundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer);
    }
    public void Jump(bool jumpInput)
    {
        if (!jumpInput && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            pState.jumping = false;
            

        }

        if (!pState.jumping)
        {
            if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                pState.jumping = true;
                

            }
            else if (!IsGrounded() && jumpCount < maxJumpCount && jumpInput)
            {
                pState.jumping = true;
                jumpCount++;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                


            }

        }
        animator.SetBool("Jumping", !IsGrounded());
       

    }
    public void UpdateJumpVariables(bool jumpInput)
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
        if (jumpInput)
        {
            jumpBufferCounter = jumpBufferFrames;
            gravityScale = jumpGScale;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime * 10;
            gravityScale = fallGScale;
        }
    }
    public IEnumerator WalkIntoNewScene(Vector2 _exitDir, float _delay)
    {

        if (_exitDir.y > 0)
        {
            rb.linearVelocity = jumpForce * _exitDir;
        }
        if (_exitDir.x > 0)
        {
            float exitValue = _exitDir.x > 0 ? 1 : -1;
            pInput.SetDirectionalInput(new Vector2(exitValue, 0));
            Move(pInput.directionalInput);

        }

        Flip(pInput.directionalInput);

        yield return new WaitForSeconds(_delay);
        pState.cutscene = false;

    }

}
