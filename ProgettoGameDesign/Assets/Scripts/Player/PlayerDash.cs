using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Dashing")]

    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] GameObject dashEffect;
    [SerializeField] float effectPosition = 0.5f;
    private PlayerMovement playerMovement;
    PlayerStateList pState;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canDash = true;
    private void OnEnable()
    {
        PlayerTransform.OnTransform += ChangeVariables;
    }
    private void OnDisable()
    {
        PlayerTransform.OnTransform -= ChangeVariables;
    }
    private void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        animator = Player.Instance.animator;
        playerMovement = GetComponent<PlayerMovement>();
    }
    void ChangeVariables()
    {
        animator = Player.Instance.animator;
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
        
        
        EffectManager.Instance.PlayOneShot(dashEffect.GetComponent<ParticleSystem>(), transform.position+Vector3.down*effectPosition);
        CameraManager.Instance.ShakeCamera(0.1f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale =   playerMovement.gravityScale;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        
    }
}
