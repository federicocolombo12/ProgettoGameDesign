using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Canvas pauseMenuCanvas;
    private void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
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
        
        pauseMenuCanvas.enabled = !pauseMenuCanvas.enabled; // Abilita il menu di pausa quando l'input è attivo
    }


}
