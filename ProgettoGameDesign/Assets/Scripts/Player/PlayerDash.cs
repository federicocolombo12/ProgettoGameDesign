using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Dashing")]

    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] GameObject dashEffect;
    private PlayerMovement playerMovement;
    PlayerStateList pState;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canDash = true;
    private void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void DoDash(bool dashed) { 
        if (canDash && !pState.dashing && dashed)
        {
            StartCoroutine(Dash());
        }
    }

    // Update is called once per frame
    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        animator.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * dashSpeed, 0);
        if (playerMovement.IsGrounded()) 
        { Instantiate(dashEffect, transform); 
        }
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale =   playerMovement.gravityScale;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        
    }
}
