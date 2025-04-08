using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] LayerMask attackableLayer;
    public Transform sideAttackTransform;
    public Transform upAttackTransform;
    public Transform downAttackTransform;
    [SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] float damage = 10;
    [SerializeField] float hitForce = 10;
    [SerializeField] GameObject slashEffect;
    [SerializeField] float timeBetweenAttack, timeSinceAttack;
    [Space(10)]
    [Header("Recoil")]
    [SerializeField] float recoilXSpeed = 1f;
    [SerializeField] float recoilYSpeed = 1f;
    [SerializeField] int recoilXSteps = 10;
    [SerializeField] int recoilYSteps = 10;
    int stepsXRecoiled = 0;
    int stepsYRecoiled = 0;
    PlayerStateList pState;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    PlayerCast playerSpell;
    private void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerSpell = GetComponent<PlayerCast>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null && pState.casting)
        {
            Debug.Log("Enemy Hit by spell");
            other.GetComponent<Enemy>().EnemyHit(playerSpell.spelldamage, (other.transform.position - transform.position).normalized, -recoilYSpeed);
        }
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
    public void Attack(bool attack, Vector2 directionalInput)
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {

            timeSinceAttack = 0;
            animator.SetTrigger("Attack");
            attack = false;
            Debug.Log("Attacco eseguito!");

            if (Mathf.Abs(directionalInput.y) < 0.3f || (directionalInput.y < 0 && playerMovement.IsGrounded()))
            {
                Debug.Log("Attacco laterale");
                int _recoilLeftOrRight = pState.lookingRight ? 1 : -1;
                Hit(sideAttackTransform, sideAttackArea, ref pState.recoilingX, Vector2.right * _recoilLeftOrRight, recoilXSpeed);
                Instantiate(slashEffect, sideAttackTransform);
            }
            else if (directionalInput.y > 0.3f)
            {
                Debug.Log("Attacco alto");
                Hit(upAttackTransform, upAttackArea, ref pState.recoilingY,Vector2.up, recoilYSpeed);
                SlashEffectAngle(slashEffect, 90, upAttackTransform);
            }
            else if (directionalInput.y < 0.3f && !playerMovement.IsGrounded())
            {
                Debug.Log("Attacco basso");
                Hit(downAttackTransform, downAttackArea, ref pState.recoilingY,Vector2.down, recoilYSpeed);
                SlashEffectAngle(slashEffect, -90, downAttackTransform);
            }
            // Attacco
        }
    }



    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoildBool,Vector2 _recoilDir, float _recoilStrenght)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        if (hits.Length > 0)
        {
            _recoildBool = true;
            for (int i = 0; i < hits.Length; i++)
            {


                hits[i].GetComponent<Enemy>().EnemyHit(
                    damage, _recoilDir, _recoilStrenght);
                if (hits[i].CompareTag("Enemy"))
                {
                    playerHealth.Mana += playerHealth.manaGain;
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
    public void Recoil(Vector2 directionalInput)
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
            playerMovement.jumpCount = 0;
        }
        else
        {
            rb.gravityScale = playerMovement.gravityScale;
        }
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
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
        if (playerMovement.IsGrounded())
        {
            StopRecoilY();
        }
    }
}
