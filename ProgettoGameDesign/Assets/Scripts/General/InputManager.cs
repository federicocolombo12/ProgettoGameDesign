using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Pattern Singleton per avere un riferimento globale (opzionale)
    public static InputManager Instance { get; private set; }

    // Eventi per gli input, ad esempio per il movimento e il salto
    public event System.Action<Vector2> OnMoveInput;
    public event System.Action<bool> OnJumpInput;
    public event System.Action<bool> OnHealInput;
    public event System.Action<bool> OnDashInput;
    public event System.Action<bool> OnAttackInput;
    public event System.Action<bool> OnCastSpellInput;
    public event System.Action<bool> OnTriggerAbility1Input;
    public event System.Action<bool> OnTriggerAbility2Input;
    public event System.Action<bool> OnInteractInput;
    public event System.Action OnMenuInput;

    public static InputSystem_Actions inputActions;
    public static event System.Action<InputActionMap> OnActionMapChanged;
    

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

        
        SwitchActionMap(inputActions.Player); // Abilita l'ActionMap Player all'avvio
        PlayerInputActionMap();
        

    }
    private void PlayerInputActionMap() {
        inputActions.Player.Move.performed += HandleMove;
        inputActions.Player.Move.canceled += HandleMoveCanceled;
        inputActions.Player.Jump.performed += HandleJump;
        inputActions.Player.Jump.canceled += HandleJumpCanceled;
        inputActions.Player.Dash.performed += HandleDash;
        inputActions.Player.Dash.canceled += HandleDashCanceled;
        inputActions.Player.Attack.performed += HandleAttack;
        inputActions.Player.Attack.canceled += HandleAttackCanceled;
        inputActions.Player.Healing.performed += HandleHeal;
        inputActions.Player.Healing.canceled += HandleHealCanceled;
        inputActions.Player.CastSpell.performed += HandleCastSpell;
        inputActions.Player.CastSpell.canceled += HandleCastSpellCanceled;
        inputActions.Player.TriggerAbility1.started += HandleTriggerAbility1;
        inputActions.Player.TriggerAbility1.canceled += HandleTriggerAbility1Canceled;
        inputActions.Player.TriggerAbility2.started += HandleTriggerAbility2;
        inputActions.Player.TriggerAbility2.canceled += HandleTriggerAbility2Canceled;
        inputActions.Player.Interact.performed += HandleInteract;
        inputActions.Player.Interact.canceled += HandleInteractCanceled;
        inputActions.Player.Menu.performed += HandleMenu;
        
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    public static void SwitchActionMap(InputActionMap _inputActionMap)
    {
        if (_inputActionMap.enabled)
        {
            Debug.LogWarning("L'ActionMap è già abilitato: " + _inputActionMap.name);
            return;
        }
        inputActions.Disable();
        Debug.Log("abilitato ActionMap: " + _inputActionMap);
       
        _inputActionMap.Enable();
        OnActionMapChanged?.Invoke(_inputActionMap);
        LogActionMapStates();

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
        
        OnJumpInput?.Invoke(true);
    }
    private void HandleJumpCanceled(InputAction.CallbackContext context)
    {
        OnJumpInput?.Invoke(false);
    }
    private void HandleDash(InputAction.CallbackContext context)
    {
        OnDashInput?.Invoke(true);
    }
    private void HandleDashCanceled(InputAction.CallbackContext context)
    {
        OnDashInput?.Invoke(false);
    }
    private void HandleAttack(InputAction.CallbackContext context)
    {
        OnAttackInput?.Invoke(true);
    }
    private void HandleAttackCanceled(InputAction.CallbackContext context)
    {
        OnAttackInput?.Invoke(false);
    }
    private void HandleHeal(InputAction.CallbackContext context)
    {
       
        OnHealInput?.Invoke(true);
    }
    private void HandleHealCanceled(InputAction.CallbackContext context)
    {
        OnHealInput?.Invoke(false);
    }
    private void HandleCastSpell(InputAction.CallbackContext context)
    {
        OnCastSpellInput?.Invoke(true);
    }
    private void HandleCastSpellCanceled(InputAction.CallbackContext context)
    {
        OnCastSpellInput?.Invoke(false);
    }
    private void HandleTriggerAbility1(InputAction.CallbackContext context)
    {
        OnTriggerAbility1Input?.Invoke(true);
    }
    private void HandleTriggerAbility1Canceled(InputAction.CallbackContext context)
    {
        OnTriggerAbility1Input?.Invoke(false);
    }
    private void HandleTriggerAbility2(InputAction.CallbackContext context)
    {
        OnTriggerAbility2Input?.Invoke(true);
    }
    private void HandleTriggerAbility2Canceled(InputAction.CallbackContext context)
    {
        OnTriggerAbility2Input?.Invoke(false);
    }
    private void HandleInteract(InputAction.CallbackContext context)
    {
        OnInteractInput?.Invoke(true);
    }
    private void HandleInteractCanceled(InputAction.CallbackContext context)
    {
        OnInteractInput?.Invoke(false);
    }
    private void HandleMenu(InputAction.CallbackContext context)
    {
        OnMenuInput?.Invoke();
    }
    public static void LogActionMapStates()
    {
        foreach (var map in inputActions.asset.actionMaps)
        {
            Debug.Log($"ActionMap {map.name} is enabled: {map.enabled}");
        }
    }


}
