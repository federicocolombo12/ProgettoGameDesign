using DG.Tweening;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    InputSystem_Actions inputActions;
    public Vector2 directionalInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpPressed { get; private set; } // nuova proprietà

    private bool previousJumpInput = false;
    public bool attack { get; private set; }
    public bool healPressed { get; private set; }
    public bool cast { get; private set; }
    public bool dashed { get; private set; }
    public bool leftTranformation { get; private set; }
    public bool rightTransformation { get; private set; }
    public bool interact { get; private set; } // Aggiunto per l'interazione
    private bool canDash = true;
    void Start()
    {
        inputActions = InputManager.inputActions;
        InputManager.Instance.OnMoveInput += HandleMoveInput;
        InputManager.Instance.OnJumpInputStarted += HandleJumpStarted;
        InputManager.Instance.OnJumpInputPerformed += HandleJumpPerformed; // Imposta jumpInput a true quando il salto inizia
        InputManager.Instance.OnJumpInputCanceled += ResetJumpInput; // Aggiunto per resettare jumpInput quando il salto finisce
        InputManager.Instance.OnDashInput += HandleDash;
        InputManager.Instance.OnAttackInput += HandleAttack;
        InputManager.Instance.OnHealInput += HandleHeal;
        InputManager.Instance.OnCastSpellInput += HandleCast;
        InputManager.Instance.OnTriggerAbility1Input += HandleTriggerAbility1;
        InputManager.Instance.OnTriggerAbility2Input += HandleTriggerAbility2;
        InputManager.Instance.OnInteractInput += HandleInteract; // Aggiunto per l'interazione
        InputManager.Instance.OnMenuInput += HandleMenuInput; // Aggiunto per il menu
    }

   
    private void OnDisable()
    {
        // Rimozione delle sottoscrizioni per evitare errori
        InputManager.Instance.OnMoveInput -= HandleMoveInput;
        InputManager.Instance.OnJumpInputStarted -= HandleJumpStarted;

        InputManager.Instance.OnDashInput -= HandleDash;
        InputManager.Instance.OnAttackInput -= HandleAttack;
        InputManager.Instance.OnHealInput -= HandleHeal;
        InputManager.Instance.OnCastSpellInput -= HandleCast;
        InputManager.Instance.OnTriggerAbility1Input -= HandleTriggerAbility1;
        InputManager.Instance.OnTriggerAbility2Input -= HandleTriggerAbility2;
        InputManager.Instance.OnInteractInput -= HandleInteract; // Aggiunto per l'interazione
        InputManager.Instance.OnMenuInput -= HandleMenuInput; // Aggiunto per il menu

    }
  
    private void HandleMoveInput(Vector2 input)
    {
        directionalInput = input;
    }

    private void HandleJumpStarted()
    {
        jumpPressed = true; // Imposta jumpPressed a true quando il salto inizia
        DOVirtual.DelayedCall(0.1f, () => jumpPressed=false); // Imposta previousJumpInput a true dopo un breve ritardo
    }
    private void HandleJumpPerformed()
    {
        jumpInput = true; // Imposta jumpInput a true quando il salto inizia
        
    }
    private void ResetJumpInput()
    {
        jumpInput = false; // Resetta jumpInput quando il salto finisce
        jumpPressed = false; // Resetta jumpPressed quando il salto finisce
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
    private void HandleInteract(bool interacted)
    {
        interact = interacted;
    }
    private void HandleMenuInput()
    {
        Debug.Log("Called from Player Input");
        InputManager.SwitchActionMap(inputActions.UI);
    }
}
