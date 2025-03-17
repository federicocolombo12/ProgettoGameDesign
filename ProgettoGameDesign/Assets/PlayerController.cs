using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("Horizontal Movement")]
    [SerializeField] float speed = 5f;
    private Vector2 moveInput;
    [Header("Jumping")]
    private float jumpInput;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckY = 0.2f;
    [SerializeField] float GroundCheckX = 0.5f;
    [SerializeField] LayerMask groundLayer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InputManager.Instance.OnMoveInput += HandleMoveInput;
        InputManager.Instance.OnJumpInput += HandleJump;
    }

   


    private void OnDisable()
    {
        // Rimozione delle sottoscrizioni per evitare errori
        InputManager.Instance.OnMoveInput -= HandleMoveInput;
        InputManager.Instance.OnJumpInput -= HandleJump;
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

    private void FixedUpdate()
    {
        Move();
        Jump();
    }
    void Move()
    {
        // Preserva la velocit� verticale (gestita dalla gravit� o da altri effetti fisici)
        Vector2 newVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;
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
        }
        if (IsGrounded()) { 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpInput * jumpForce);
        }

    }

}
