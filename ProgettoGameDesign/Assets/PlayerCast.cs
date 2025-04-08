using UnityEngine;
using System.Collections;

public class PlayerCast : MonoBehaviour
{
    [Header("Spell Casting")]
    [SerializeField] float manaSpellCost = 0.3f;
    [SerializeField] float timeBetweenCast = 0.5f;
    float timeSinceCast;
    public float spelldamage;
    [SerializeField] float downSpellForce;

    [SerializeField] GameObject sideSpellfireBall;
    [SerializeField] GameObject downSpellfireBall;
    [SerializeField] GameObject upSpellfireBall;
    private PlayerStateList pState;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();


    }
    public void CastSpell(bool cast, bool grounded, Vector2 directionalInput)
    {
        if (cast && timeSinceCast >= timeBetweenCast && playerHealth.Mana >= manaSpellCost)
        {
            timeSinceCast = 0;
            pState.casting = true;
            cast = false;
            StartCoroutine(CastCoroutine(directionalInput, grounded));
        }
        else
        {
            timeSinceCast += Time.deltaTime;
        }

        if (grounded)
        {
            downSpellfireBall.SetActive(false);
        }

        if (downSpellfireBall.activeInHierarchy)
        {
            rb.linearVelocity += downSpellForce * Vector2.down;
        }

    }

    IEnumerator CastCoroutine(Vector2 directionalInput, bool grounded)
    {
        animator.SetBool("Casting", true);
        yield return new WaitForSeconds(0.15f);
        if (Mathf.Abs(directionalInput.y) < 0.3f || (directionalInput.y < 0 && grounded))
        {
            GameObject fireball = Instantiate(sideSpellfireBall,playerAttack.sideAttackTransform.position, Quaternion.identity);
            if (pState.lookingRight)
            {
                fireball.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                fireball.transform.eulerAngles = new Vector2(fireball.transform.eulerAngles.x, 180);
            }
            pState.recoilingX = true;
        }
        else if (directionalInput.y > 0.3f)
        {
            Instantiate(upSpellfireBall, transform);
            rb.linearVelocity = Vector2.zero;
        }
        else if (directionalInput.y < 0.3f && !grounded)
        {
            downSpellfireBall.SetActive(true);
        }
        playerHealth.Mana -= manaSpellCost;
        yield return new WaitForSeconds(0.35f);
        animator.SetBool("Casting", false);
        pState.casting = false;
    }
}
