using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Pattern Singleton per avere un riferimento globale (opzionale)
    public static InputManager Instance { get; private set; }

    // Eventi per gli input, ad esempio per il movimento e il salto
    public event System.Action<Vector2> OnMoveInput;
    public event System.Action<float> OnJumpInput;
    public event System.Action<bool> OnHealInput;
    public event System.Action OnDashInput;
    public event System.Action OnAttackInput;
    public event System.Action OnCastSpellInput;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        // Implementazione Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        // Sottoscrizione agli eventi di input
        inputActions.Player.Move.performed += HandleMove;
        inputActions.Player.Move.canceled += HandleMoveCanceled;
        inputActions.Player.Jump.performed += HandleJump;
        inputActions.Player.Jump.canceled += HandleJumpCanceled;
        inputActions.Player.Dash.started += HandleDash;
        inputActions.Player.Attack.performed += HandleAttack;
        inputActions.Player.Healing.performed += HandleHeal;
        inputActions.Player.Healing.canceled += HandleHealCanceled;
        inputActions.Player.CastSpell.performed += HandleCastSpell;
        inputActions.Player.CastSpell.canceled += HandleCastSpellCanceled;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        Vector2 moveValue = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(moveValue);
    }

    private void HandleMoveCanceled(InputAction.CallbackContext context)
    {
        // Quando il movimento si ferma, emettiamo un vettore zero
        OnMoveInput?.Invoke(Vector2.zero);
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        float jumpValue = context.ReadValue<float>();
        OnJumpInput?.Invoke(jumpValue);
    }
    private void HandleJumpCanceled(InputAction.CallbackContext context)
    {
        OnJumpInput?.Invoke(0);
    }
    private void HandleDash(InputAction.CallbackContext context)
    {
        OnDashInput?.Invoke();
    }
    private void HandleDashCanceled(InputAction.CallbackContext context)
    {
        OnDashInput?.Invoke();
    }
    private void HandleAttack(InputAction.CallbackContext context)
    {
        OnAttackInput?.Invoke();
    }
    private void HandleAttackCanceled(InputAction.CallbackContext context)
    {
        //OnAttackInput?.Invoke();
    }
    private void HandleHeal(InputAction.CallbackContext context)
    {
        bool healValue = context.ReadValueAsButton();
        OnHealInput?.Invoke(healValue);
    }
    private void HandleHealCanceled(InputAction.CallbackContext context)
    {
        OnHealInput?.Invoke(false);
    }
    private void HandleCastSpell(InputAction.CallbackContext context)
    {
        OnCastSpellInput?.Invoke();
    }
    private void HandleCastSpellCanceled(InputAction.CallbackContext context)
    {
        //OnCastSpellInput?.Invoke(false);
    }
}
