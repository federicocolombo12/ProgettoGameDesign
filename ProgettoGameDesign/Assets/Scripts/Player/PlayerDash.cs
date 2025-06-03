using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerDash : MonoBehaviour
{
    [Header("Dashing")]

    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] GameObject dashEffect;
    [SerializeField] GameObject dashTrail;
    [SerializeField] Transform dashPosition;
    [SerializeField] float effectPosition = 0.5f;
    private PlayerMovement playerMovement;
    PlayerStateList pState;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] AnimationCurve gravityCurve;
    private bool canDash = true;
    [SerializeField] private float shakeIntensity;

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
        pState = Player.Instance.pState;
        rb = Player.Instance.rb;
        animator = Player.Instance.animator;
        playerMovement = Player.Instance.playerMovement;

    }
    void ChangeVariables()
    {
        animator = Player.Instance.animator;
    }

    public void DoDash(bool dashed)
    {
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



        while (rb.gravityScale != 0)
        {
            rb.gravityScale = 0;
            yield return null;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x * dashSpeed, 0);


        EffectManager.Instance.PlayOneShot(dashEffect.GetComponent<ParticleSystem>(), transform.position + Vector3.down * effectPosition);
        ChangeAlpha();
        ParticleSpawn();

        CameraManager.Instance.ShakeCamera(shakeIntensity);
        yield return new WaitForSeconds(dashTime);

        pState.dashing = false;
        rb.gravityScale = playerMovement.gravityScale;


        yield return new WaitForSeconds(dashCooldown);

        canDash = true;


    }
    void ChangeAlpha()
    {
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.DOColor(Color.black, 0.1f).OnComplete(() =>
            {
                spriteRenderer.DOColor(Color.white, 0.1f);
            });
        }

    }
    void ParticleSpawn()
    {
        if (dashTrail != null)
        {
            GameObject trail = Instantiate(dashTrail, dashPosition.position, Quaternion.identity, dashPosition);
            
            Destroy(trail, dashCooldown);
        }
    }
}
