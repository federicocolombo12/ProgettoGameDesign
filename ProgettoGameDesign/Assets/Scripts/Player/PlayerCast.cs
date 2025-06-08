using UnityEngine;
using UnityEngine.Events;
using System.Linq;

using System.Collections;

public class PlayerCast : MonoBehaviour
{
    [Header("Spell Casting")]
    [SerializeField] float manaSpellCost = 0.3f;
    [SerializeField] float timeBetweenCast = 0.5f;
    float timeSinceCast;
    public float spelldamage;
    

    [SerializeField] GameObject sideSpellfireBall;
    
    private PlayerStateList pState;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private ParticleSystem castEffect;

    private void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = Player.Instance.rb;
        animator = Player.Instance.animator;
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();


    }

    public void CastSpell(bool cast, bool grounded, Vector2 directionalInput)
    {
        if (!Player.Instance.playerTransformation.abilities.Contains(PlayerTransformation.AbilityType.SpellCasting))
        {
            return;
        }
        if (cast && timeSinceCast >= timeBetweenCast && playerHealth.Mana >= manaSpellCost)
        {
            timeSinceCast = 0;
            pState.casting = true;
            cast = false;
            animator = Player.Instance.animator;

            StartCoroutine(CastCoroutine(directionalInput, grounded));
        }
        else
        {
            timeSinceCast += Time.deltaTime;
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
        
        playerHealth.Mana -= manaSpellCost;
        EffectManager.Instance.PlayOneShot(castEffect, playerAttack.sideAttackTransform.position);
        yield return new WaitForSeconds(0.35f);
        animator.SetBool("Casting", false);
        pState.casting = false;
    }
}
