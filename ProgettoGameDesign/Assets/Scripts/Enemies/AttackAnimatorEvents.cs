using TMPro.Examples;
using UnityEngine;

public class AttackAnimatorEvents : MonoBehaviour
{
    [SerializeField] Collider2D attackCollider;
    [SerializeField] Collider2D dashCollider;
    [SerializeField] Collider2D baseCollider;
    
    private void OnAttackStart()
    {

        dashCollider.enabled = true;
        baseCollider.enabled = false;
       
        
    }

    // Update is called once per frame
    private void OnAttackEnd()
    {
        dashCollider.enabled = false;
        baseCollider.enabled = true;
    }
}
