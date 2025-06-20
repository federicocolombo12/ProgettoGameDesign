using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference quitActionReference;

    [Header("Opacity")]
    [SerializeField] private float targetAlpha = 0.8f;

    [Header("UI References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject graphicsMenu;

    private Canvas pauseMenuCanvas;
    private CanvasGroup pauseMenuCanvasGroup;
    private bool isPaused = false;

    private enum MenuState
    {
        None,
        Main,
        Controls,
        Audio,
        Graphics
    }

    private MenuState currentMenu = MenuState.None;

    private void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvasGroup = GetComponent<CanvasGroup>();

        pauseMenuCanvas.enabled = false;
    }

    private void OnEnable()
    {
        InputManager.OnActionMapChanged += HandleActionMapChange;
        quitActionReference.action.performed += OnCancelPressed;
    }

    private void OnDisable()
    {
        InputManager.OnActionMapChanged -= HandleActionMapChange;
        quitActionReference.action.performed -= OnCancelPressed;
    }

    private void HandleActionMapChange(InputActionMap map)
    {
        if (map.name == "UI")
        {
            EnablePauseMenu();
        }
    }

    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (!isPaused) return;

        switch (currentMenu)
        {
            case MenuState.Controls:
            case MenuState.Audio:
            case MenuState.Graphics:
                ShowMainMenu();
                return; //  INTERROMPI subito! NON continuare
            case MenuState.Main:
                DisablePauseMenu(); // Riprendi il gioco solo se nel menu principale
                break;
        }
    }


    private void EnablePauseMenu()
    {
        isPaused = true;
        pauseMenuCanvas.enabled = true;
        pauseMenuCanvasGroup.alpha = targetAlpha;
        ShowMainMenu(); // default view
        Time.timeScale = 0f;
    }

    private void DisablePauseMenu()
    {
        isPaused = false;
        pauseMenuCanvas.enabled = false;
        pauseMenuCanvasGroup.alpha = 0f;
        Time.timeScale = 1f;
        InputManager.SwitchActionMap(InputManager.inputActions.Player);
        currentMenu = MenuState.None;
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        controlsMenu.SetActive(false);
        audioMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        currentMenu = MenuState.Main;
    }

    public void ShowControlsMenu()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
        currentMenu = MenuState.Controls;
    }

    public void ShowAudioMenu()
    {
        mainMenu.SetActive(false);
        audioMenu.SetActive(true);
        currentMenu = MenuState.Audio;
    }

    public void ShowGraphicsMenu()
    {
        mainMenu.SetActive(false);
        graphicsMenu.SetActive(true);
        currentMenu = MenuState.Graphics;
    }
}
