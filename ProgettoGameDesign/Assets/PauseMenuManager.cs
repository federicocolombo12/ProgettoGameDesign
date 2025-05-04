using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Canvas pauseMenuCanvas;
    private void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvas.enabled = false; // Assicurati che il menu sia inizialmente disabilitato
    }
    private void OnEnable()
    {
        InputManager.OnActionMapChanged += EnableMenu;
    
    }
    private void OnDisable()
    {
        InputManager.OnActionMapChanged -= EnableMenu;
    }
    void EnableMenu(InputActionMap uiMap)
    {
        
        pauseMenuCanvas.enabled = true; // Abilita il menu di pausa quando l'input è attivo
    }


}
