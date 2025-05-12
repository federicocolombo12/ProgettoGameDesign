using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialCanvasController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Image powerImage;
    CanvasGroup tutorialCanvas;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] TextMeshProUGUI tutorialTitle;
    [SerializeField] TutorialEvents tutorialEvents;
    [SerializeField] InputActionReference cancelActionReference;
    [SerializeField] GameObject gameCanvas;
    private void OnEnable()
    {
        tutorialEvents.OnTriggerTutorialRequested += TriggerTutorial;
        cancelActionReference.action.performed += HandleCancel;
    }
    private void OnDisable()
    {
        tutorialEvents.OnTriggerTutorialRequested -= TriggerTutorial;
        cancelActionReference.action.performed -= HandleCancel;
    }

    void Start()
    {
        powerImage = GetComponentInChildren<Image>();
        tutorialCanvas = GetComponent<CanvasGroup>();
        tutorialCanvas.alpha = 0;
        
        
    }

    // Update is called once per frame
    void TriggerTutorial(Tutorial tutorialMaterial) {
        gameCanvas.SetActive(false);
        StartCoroutine(TutorialCoroutine(tutorialMaterial));
    }
    void HandleCancel(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        tutorialCanvas.DOFade(0, 0.5f);
        InputManager.SwitchActionMap(InputManager.inputActions.Player);
        Time.timeScale = 1;
        gameCanvas.SetActive(true);
    }
    IEnumerator TutorialCoroutine(Tutorial _tutorialMaterial) {
        tutorialCanvas.DOFade(1, 0.5f);
        tutorialText.text = _tutorialMaterial.tutorialText;
        tutorialTitle.text = _tutorialMaterial.tutorialTitle;
        powerImage.sprite = _tutorialMaterial.tutorialImage;
        InputManager.SwitchActionMap(InputManager.inputActions.Tutorial);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }
}
