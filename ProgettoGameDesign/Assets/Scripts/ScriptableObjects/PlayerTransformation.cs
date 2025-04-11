using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTransformation", menuName = "Scriptable Objects/PlayerTransformation")]

public class PlayerTransformation : ScriptableObject
{
    public enum AbilityType
    {
        None,
        WallSlide,
        GrapplingHook,
        miniMe,
    }
    public float maxHealth;
    public float moveSpeed;
    public float jumpForce;
    public float jumpCount;
    public float dashSpeed;
    public AbilityType abilityType;
    public float attackDamage;
    public float spellDamage;
    
}
