using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Rigidbody2D rb;
    private Animator animator;
    public PlayerStateList pState;
    
    private SpriteRenderer sr;
    public PlayerMovement playerMovement { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public PlayerAttack playerAttack { get; private set; }
    public PlayerHealth playerHealth { get; private set; }
    public PlayerCast playerSpell { get; private set; }
    public PlayerDash playerDash { get; private set; }
    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        playerSpell = GetComponent<PlayerCast>();
        playerDash = GetComponent<PlayerDash>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.UpdateJumpVariables(playerInput.jumpInput);
        playerMovement.Move(playerInput.directionalInput);
        playerMovement.Jump(playerInput.jumpInput);
        playerDash.DoDash(playerInput.dashed);
        
    }
}
