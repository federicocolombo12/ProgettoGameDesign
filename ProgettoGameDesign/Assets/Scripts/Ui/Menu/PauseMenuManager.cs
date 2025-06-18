using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using DG.Tweening;


public class PauseMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Canvas pauseMenuCanvas;
    CanvasGroup pauseMenuCanvasGroup;
    [SerializeField] InputActionReference quitActionReference;
    [SerializeField] float targetAlpha = 0.8f; // Opacit�� finale del menu di pausa
    private void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvas.enabled = false; // Disabilita il menu di pausa all'inizio+
        pauseMenuCanvasGroup = GetComponent<CanvasGroup>();
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
            
            // Esegui logica per tornare al gameplay, se necessario
        }
        else if (map.name == "UI")
        {
            
            EnableMenu(map);
        }
    }

    void EnableMenu(InputActionMap uiMap)
    {

        pauseMenuCanvas.enabled = true; // Abilita il menu di pausa quando l'input � attivo
        pauseMenuCanvasGroup.alpha = targetAlpha; // Imposta l'opacit�� iniziale a 0
        Time.timeScale= 0f; // Ferma il tempo di gioco
    }   
    void DisableMenu(InputAction.CallbackContext callback)
    {
        pauseMenuCanvasGroup.alpha = 0f; // Imposta l'opacit�� a 0 per nascondere il menu di pausa
        Canvas pauseMenuCanvas = GetComponent<Canvas>();
        if (pauseMenuCanvas == null)
        {
            Debug.LogError("Il Canvas non � stato trovato.");
            return;
        }
        pauseMenuCanvas.enabled = false; // Disabilita il menu di pausa quando l'input � attivo
        InputManager.SwitchActionMap(InputManager.inputActions.Player);
        Time.timeScale = 1; // Torna alla mappa di input del giocatore

    }
   


}
