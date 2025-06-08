using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Rigidbody2D rb;

    public Animator animator;
    public PlayerStateList pState;
    public PlayerTransformation playerTransformation;

    public SpriteRenderer sr;
    public PlayerMovement playerMovement { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public PlayerAttack playerAttack { get; private set; }
    public PlayerHealth playerHealth { get; private set; }
    public PlayerCast playerSpell { get; private set; }
    public PlayerDash playerDash { get; private set; }
    public PlayerTransform playerTransform { get; private set; }
    public PlayerInteract playerInteract { get; private set; }
    public PlayerCameraHandler playerCameraHandler { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        playerSpell = GetComponent<PlayerCast>();
        playerDash = GetComponent<PlayerDash>();
        playerTransform = GetComponent<PlayerTransform>();
        playerInteract = GetComponent<PlayerInteract>();
        playerCameraHandler = GetComponent<PlayerCameraHandler>();
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {


        playerMovement.Stick(playerInput.directionalInput, playerInput.jumpInput);
        if (pState.dashing) return;

        playerHealth.RestoreTimeScale();
        if (pState.hooked) return;
        playerMovement.UpdateJumpVariables(playerInput.jumpInput);
        if (pState.sticking) return;


        playerHealth.FlashWhileInvincible();


        playerMovement.Move(playerInput.directionalInput);


        playerHealth.Heal(playerInput.healPressed);
        if (pState.healing)
        {
            return;
        }
        playerMovement.Jump(playerInput.jumpPressed);

        playerDash.DoDash(playerInput.dashed);
        playerAttack.Attack(playerInput.attack, playerInput.directionalInput);
        playerAttack.Recoil(playerInput.directionalInput);
        playerSpell.CastSpell(playerInput.cast, playerMovement.IsGrounded(), playerInput.directionalInput);
        playerTransform.HandleTransform(playerInput.leftTranformation, playerInput.rightTransformation);
        playerInteract.PlayerCheckForInteractables(playerInput.interact);

    }

    void LateUpdate()
    {
        playerCameraHandler.CameraYDamping();
    }



    public void RespawnAt(Vector2 position)
    {
        transform.position = position;
        rb.linearVelocity = Vector2.zero;
        
        playerHealth.Respawned();

        //eventuali altre inizializzazioni del player necessarie dopo il respawn ( l'avevo pensata cosi)
    }
    

}
