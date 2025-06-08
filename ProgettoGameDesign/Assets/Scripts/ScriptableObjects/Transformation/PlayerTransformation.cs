using UnityEngine;
using UnityEngine.Animations;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayerTransformation", menuName = "Scriptable Objects/PlayerTransformation")]
public class PlayerTransformation : ScriptableObject
{
    public enum AbilityType
    {
        None,
        WallSlide,
        GrapplingHook,
        doubleJump,
        ChargingRockBreaker,
        SpellCasting
    }
    [InfoBox("Aggiungi qui le abilità che questa trasformazione possiede.")]
    public AbilityType[] abilities = new AbilityType[]
    {
        AbilityType.None,
        // Per aggiungere altre abilità, scrivi qui sotto, ad esempio:
        // AbilityType.WallSlide,
        // AbilityType.GrapplingHook,
        // AbilityType.doubleJump,
        // AbilityType.ChargingRockBreaker,
    };

    // Metodo per aggiungere un'abilità via codice
    public void AddAbility(AbilityType ability)
    {
        if (System.Array.IndexOf(abilities, ability) < 0)
        {
            var list = new System.Collections.Generic.List<AbilityType>(abilities);
            list.Add(ability);
            abilities = list.ToArray();
        }
    }

    public bool isUnlocked = false;
    [InfoBox("Questa trasformazione è sbloccata quando il giocatore ottiene un certo numero di gemme. Se non è sbloccata, il giocatore non può usarla.")]

    [Title("General Info")]
    [LabelWidth(120)]
    public int index;

    [EnumToggleButtons]
    [Tooltip("Tipo di abilità speciale che questa trasformazione sblocca.")]
    public AbilityType abilityType;

    [TabGroup("Stats"), LabelWidth(120)]
    public int maxHealth;

    [TabGroup("Stats"), LabelWidth(120)]
    public float damageMultiplier;

    [TabGroup("Stats"), LabelWidth(120)]
    public float moveSpeed;

    [TabGroup("Stats"), LabelWidth(120)]
    public float jumpForce;

    [TabGroup("Stats"), LabelWidth(120)]
    public int jumpCount;

    [TabGroup("Stats"), LabelWidth(120)]
    public float dashSpeed;

    [TabGroup("Stats"), LabelWidth(120)]
    public float attackDamage;

    [TabGroup("Stats"), LabelWidth(120)]
    public float attackSpeed;

    [TabGroup("Stats"), LabelWidth(120)]
    public float spellDamage;

    [TabGroup("Visual"), PreviewField(70)]
    [Tooltip("Sprite base da usare per il personaggio in questa trasformazione.")]
    public Sprite baseSprite;

    [TabGroup("Visual")]
    [Tooltip("Animator Controller da usare per questa trasformazione.")]
    public RuntimeAnimatorController animatorController;
    [TabGroup("Visual"), LabelWidth(120)]
    public Color color;
    [TabGroup("Visual")]
    public Vector3 transformationScale = Vector3.one;

    [TabGroup("Collider"), LabelWidth(120)]
    [Tooltip("Dimensione del box collider per questa trasformazione.")]
    public Vector2 colliderSize;

    [TabGroup("Collider"), LabelWidth(120)]
    [Tooltip("Offset del box collider per questa trasformazione.")]
    public Vector2 colliderOffset;

    [Button("Get Ground Check Pos"), GUIColor(0.3f, 0.8f, 1f)]
    public Vector3 GetGroundCheckPosition()
    {
        if (Player.Instance == null)
        {
            Debug.LogWarning("Player.Instance non è assegnato.");
            return Vector3.zero;
        }

        return new Vector3(
            Player.Instance.transform.position.x,
            Player.Instance.transform.position.y -
            (colliderSize.y * Player.Instance.transform.localScale.x) / 2 +
            colliderOffset.y,
            0
        );
    }
}
