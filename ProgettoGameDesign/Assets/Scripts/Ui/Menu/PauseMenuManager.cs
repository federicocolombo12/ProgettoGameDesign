using UnityEngine;
using UnityEngine.InputSystem;


public class PauseMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Canvas pauseMenuCanvas;
    [SerializeField] InputActionReference quitActionReference;
    private void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvas.enabled = false; // Disabilita il menu di pausa all'inizio
    }
    private void OnEnable()
    {
        InputManager.OnActionMapChanged += HandleActionMapChange;
        quitActionReference.action.performed += DisableMenu;
    
    }
    private void OnDisable()
    {
        InputManager.OnActionMapChanged -= EnableMenu;
        quitActionReference.action.performed -= DisableMenu;
    }
    void HandleActionMapChange(InputActionMap map)
    {
        if (map.name == "Player")
        {
            Debug.Log("Abilitata la mappa Player (uscita dal menu)");
            // Esegui logica per tornare al gameplay, se necessario
        }
        else if (map.name == "UI")
        {
            Debug.Log("Abilitata la mappa UI (menu di pausa)");
            EnableMenu(map);
        }
    }

    void EnableMenu(InputActionMap uiMap)
    {

        pauseMenuCanvas.enabled = true; // Abilita il menu di pausa quando l'input è attivo
    }
    void DisableMenu(InputAction.CallbackContext callback)
    {
        Debug.Log("Disabilitato il menu di pausa");
        Canvas pauseMenuCanvas = GetComponent<Canvas>();
        if (pauseMenuCanvas == null)
        {
            Debug.LogError("Il Canvas non è stato trovato.");
            return;
        }
        pauseMenuCanvas.enabled = false; // Disabilita il menu di pausa quando l'input è attivo
        InputManager.SwitchActionMap(InputManager.inputActions.Player); // Torna alla mappa di input del giocatore
        
    }
   


}
