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
        ChargingRockBreaker,
    }
    public int maxHealth;
    public float moveSpeed;
    public float jumpForce;
    public int jumpCount;
    public float dashSpeed;
    public AbilityType abilityType;
    public float attackDamage;
    public float spellDamage;
    public Sprite baseSprite;
    public Vector2 colliderSize;
    public Vector2 colliderOffset;
    public RuntimeAnimatorController animatorController;
    public Vector3 transformationScale = Vector3.one;


}
