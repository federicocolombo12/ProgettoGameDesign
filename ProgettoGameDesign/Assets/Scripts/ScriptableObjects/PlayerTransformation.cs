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
    public int index;
    public int maxHealth;
    public float damageMultiplier;
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
    public Vector3 GetGroundCheckPosition() {
        // Calculate the ground check position based on the collider size and offset
        Vector3 groundCheckPosition = new Vector3(Player.Instance.transform.position.x,
            Player.Instance.transform.position.y- 
            (colliderSize.y * Player.Instance.transform.localScale.x) / 2 + colliderOffset.y, 0);
        return groundCheckPosition;
    }

}
