using TMPro.Examples;
using UnityEngine;

public class AttackAnimatorEvents : MonoBehaviour
{
    [SerializeField] Collider2D attackCollider;
    
    private void OnAttackStart()
    {

        attackCollider.enabled = true;
       
        
    }

    // Update is called once per frame
    private void OnAttackEnd()
    {
        attackCollider.enabled = false;
    }
}
