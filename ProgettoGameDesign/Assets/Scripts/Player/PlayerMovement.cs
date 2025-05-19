using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput pInput;
    

    [TabGroup("Movement"), LabelText("Speed")]
    [SerializeField] private float speed = 5f;

    [TabGroup("Camera Stuff"), LabelText("Camera Follow Object")]
    private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowGO;

    [TabGroup("Jumping"), LabelText("Jump Force")]
    [SerializeField] private float jumpForce = 10f;

    

    [TabGroup("Jumping"), LabelText("Jump Buffer Frames")]
    [SerializeField] private float jumpBufferFrames;
    private float jumpBufferCounter = 0;

    [TabGroup("Jumping"), LabelText("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter = 0;

    [TabGroup("Jumping"), LabelText("Max Jump Count")]
    [SerializeField] private int maxJumpCount = 2;

    [TabGroup("Jumping"), LabelText("Jump Gravity Scale")]
    [SerializeField] private float jumpGScale = 2f;

    [TabGroup("Jumping"), LabelText("Fall Gravity Scale")]
    [SerializeField] private float fallGScale = 0.5f;

    [TabGroup("Jumping"), ReadOnly, LabelText("Current Jump Count")]
    public int jumpCount = 0;

    [TabGroup("Jumping"), LabelText("LandEffect")]
    [SerializeField] private ParticleSystem landEffect;
    [TabGroup("Jumping"), LabelText("Land Effect Position")]
    [SerializeField] private float landPosition=1f;

    [TabGroup("Ground Check")]
    [SerializeField] private Transform groundCheck;

    [TabGroup("Ground Check"), LabelText("Check Distance Y")]
    [SerializeField] private float groundCheckY = 0.2f;

    [TabGroup("Ground Check"), LabelText("Check Offset X")]
    [SerializeField] private float GroundCheckX = 0.5f;

    [TabGroup("Ground Check")]
    [SerializeField] private LayerMask groundLayer;

    [TabGroup("Wall Jump")]
    [SerializeField] private LayerMask wallLayer;

    [TabGroup("Wall Jump"), LabelText("Stick Gravity Scale")]
    [SerializeField] private float stickGravityScale = 0.1f;

    [TabGroup("Wall Jump"), LabelText("Wall Speed")]
    [SerializeField] private float wallSpeed = 2f;

    [TabGroup("Wall Jump"), LabelText("Stick Timer Max")]
    [SerializeField] private float stickTimerMax = 0.5f;
    private float stickTimer = 0;

    [TabGroup("Runtime"), ReadOnly]
    private Rigidbody2D rb;

    [TabGroup("Runtime"), ReadOnly]
    private Animator animator;

    [TabGroup("Runtime"), ReadOnly]
    private PlayerStateList pState;

    [TabGroup("Runtime"), ReadOnly]
    public float gravityScale { get; private set; }
    private void OnEnable()
    {
        PlayerTransform.OnTransform += UpdateVariables;
    }
    private void OnDisable()
    {
        PlayerTransform.OnTransform -= UpdateVariables;
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = Player.Instance.animator;
        pState = GetComponent<PlayerStateList>();
        gravityScale = rb.gravityScale;
        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
    }
    

    // Update is called once per frame
    void UpdateVariables()
    {
        animator = Player.Instance.animator;
        speed = Player.Instance.playerTransformation.moveSpeed;
        jumpForce = Player.Instance.playerTransformation.jumpForce;
        maxJumpCount = Player.Instance.playerTransformation.jumpCount;
    }
    private void TurnCheck(Vector2 directionalInput)
    {
        if (directionalInput.x>0 && !pState.lookingRight)
        {
            Flip(directionalInput);
        }
        else if (directionalInput.x < 0 && pState.lookingRight)
        {
            Flip(directionalInput);
        }
    }
    void Flip(Vector2 directionalInput)
    {
        if (pState.lookingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x,180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            pState.lookingRight = !pState.lookingRight;
            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            pState.lookingRight = !pState.lookingRight;
            cameraFollowObject.CallTurn();
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
        if (directionalInput.x>0 || directionalInput.x<0)
        {
           TurnCheck(directionalInput);
        }
        
        
    }
    public bool IsGrounded()
    {

        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckY, groundLayer) ||
            Physics2D.Raycast(groundCheck.position + new Vector3(GroundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer)
            || Physics2D.Raycast(groundCheck.position + new Vector3(-GroundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer);
    }

    public bool IsTouchingStickyWall()
    {
        Debug.DrawRay(transform.position, Vector2.right * (pState.lookingRight ? 1 : -1), Color.red);
        return Physics2D.Raycast(transform.position, Vector2.right * (pState.lookingRight ? 1 : -1), 2f, wallLayer);
        
    }

    public void Stick(Vector2 directionalInput, bool jumpInput)
    {
        if (IsTouchingStickyWall())
        {
            pState.sticking = true;
            rb.gravityScale = stickGravityScale;
            Vector2 newVelocity = new Vector2(rb.linearVelocity.x, wallSpeed* directionalInput.y);
            rb.linearVelocity = newVelocity;
            
            
           
            
            
            if (jumpInput&&directionalInput.x!=0)
            {
                
                pState.sticking = false;
                float jumpDirection = pState.lookingRight ? -1 : 1;
                rb.linearVelocity = new Vector2(jumpDirection*rb.linearVelocity.x, jumpForce);
                
                
            }
            

        }
        else
        {
            if (!pState.hooked)
            {
                pState.sticking = false;
                rb.gravityScale = gravityScale;
            }
            
        }
    }
    public void Jump(bool jumpInput)
    {
        if (!IsTouchingStickyWall())
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
        else
        {
            if (Player.Instance.playerInput.directionalInput.x != 0)
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
        }
        
       

    }
    private bool wasGrounded = false;

    public void UpdateJumpVariables(bool jumpInput)
    {
        bool isGrounded = IsGrounded();

        if (isGrounded && !wasGrounded)
        {
            OnLand(); // Chiamata quando il giocatore atterra
        }

        wasGrounded = isGrounded;

        if (isGrounded)
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            jumpCount = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpInput && !pState.hooked)
        {
            jumpBufferCounter = jumpBufferFrames;
            gravityScale = jumpGScale;
            
        }
        else
        {
            if (!pState.hooked)
            {
                jumpBufferCounter -= Time.deltaTime * 10;
                gravityScale = fallGScale;
            }
        }
    }

    
    private void OnLand()
    {
        EffectManager.Instance.PlayOneShot(landEffect, transform.position+Vector3.down*landPosition);
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
