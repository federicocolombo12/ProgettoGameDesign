using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 directionalInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool attack { get; private set; }
    public bool healPressed { get; private set; }
    public bool cast { get; private set; }
    public bool dashed { get; private set; }
    public bool leftTranformation { get; private set; }
    public bool rightTransformation { get; private set; }
    private bool canDash = true;
    void Start()
    {
        InputManager.Instance.OnMoveInput += HandleMoveInput;
        InputManager.Instance.OnJumpInput += HandleJump;

        InputManager.Instance.OnDashInput += HandleDash;
        InputManager.Instance.OnAttackInput += HandleAttack;
        InputManager.Instance.OnHealInput += HandleHeal;
        InputManager.Instance.OnCastSpellInput += HandleCast;
        InputManager.Instance.OnTriggerAbility1Input += HandleTriggerAbility1;
        InputManager.Instance.OnTriggerAbility2Input += HandleTriggerAbility2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        // Rimozione delle sottoscrizioni per evitare errori
        InputManager.Instance.OnMoveInput -= HandleMoveInput;
        InputManager.Instance.OnJumpInput -= HandleJump;

        InputManager.Instance.OnDashInput -= HandleDash;
        InputManager.Instance.OnAttackInput -= HandleAttack;
        InputManager.Instance.OnHealInput -= HandleHeal;
        InputManager.Instance.OnCastSpellInput -= HandleCast;
        InputManager.Instance.OnTriggerAbility1Input -= HandleTriggerAbility1;
        InputManager.Instance.OnTriggerAbility2Input -= HandleTriggerAbility2;

    }
    private void HandleMoveInput(Vector2 input)
    {
        directionalInput = input;
    }

    private void HandleJump(bool jump)
    {
        // Logica di salto, ad esempio:
        Debug.Log("Salto eseguito!");

        jumpInput = jump;

    }
    private void HandleDash(bool dash)
    {
        
            dashed = dash;
            //StartCoroutine(Dash());
        


    }
    private void HandleAttack(bool attacked)
    {
        attack = attacked;
    }
    private void HandleHeal(bool healValue)
    {
        healPressed = healValue;
    }

    private void HandleCast(bool casted)
    {
        //if (Mana >= manaSpellCost)
        //{
            cast = casted;
        //}

    }
    void ResetDash()
    {

        //if (IsGrounded())
        {
            dashed = false;
        }
    }
    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }
    private void HandleTriggerAbility1(bool leftTransform)
    {
        leftTranformation = leftTransform;

        
    }
    private void HandleTriggerAbility2(bool rightTransform)
    {
        rightTransformation = rightTransform;
    }
}
