using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialCanvasController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Image powerImage;
    [SerializeField] Image buttonImage;
    [SerializeField] Image secondButtonImage;
    [SerializeField] Image TransformImage;
    [SerializeField] Image TransformImage2;
   
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
        
        tutorialCanvas = GetComponent<CanvasGroup>();
        tutorialCanvas.alpha = 0;
        
        
    }

    // Update is called once per frame
    void TriggerTutorial(Tutorial tutorialMaterial) {
        StartCoroutine(TutorialCoroutine(tutorialMaterial));
    }
    void HandleCancel(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        tutorialCanvas.DOFade(0, 0.5f);
        InputManager.SwitchActionMap(InputManager.inputActions.Player);
        Time.timeScale = 1;
    }
    IEnumerator TutorialCoroutine(Tutorial _tutorialMaterial) {
        tutorialCanvas.DOFade(1, 0.5f);
        tutorialText.text = _tutorialMaterial.tutorialText;
        tutorialTitle.text = _tutorialMaterial.tutorialTitle;
        powerImage.sprite = _tutorialMaterial.tutorialImage;
        buttonImage.sprite = _tutorialMaterial.tastoDaPremere;
        if (_tutorialMaterial.secondTastoDaPremere== null)
        {
            secondButtonImage.sprite = null;
            secondButtonImage.color = new Color(1, 1, 1, 0); // alpha 0 = trasparente
        }
        else
        {
            secondButtonImage.sprite = _tutorialMaterial.secondTastoDaPremere;
            secondButtonImage.color = Color.white; // alpha 1 = opaco
        }
        if(_tutorialMaterial.transformImage == null)
        {
            TransformImage.sprite = null;
            TransformImage.color = new Color(1, 1, 1, 0); // alpha 0 = trasparente
            TransformImage2.sprite = null;
            TransformImage2.color = new Color(1, 1, 1, 0); // alpha 0 = trasparente
        }
        else
        {
            TransformImage.sprite = _tutorialMaterial.transformImage;
            TransformImage.color = _tutorialMaterial.ColorTransformImage; // alpha 1 = opaco
            TransformImage2.sprite = _tutorialMaterial.transformImage;
            TransformImage2.color = _tutorialMaterial.ColorTransformImage; // alpha 1 = opaco
        }

        InputManager.SwitchActionMap(InputManager.inputActions.Tutorial);
        
        
        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + 0.5f) {
            yield return null; // Aspetta il frame successivo
        }

        Time.timeScale = 0; // Sospende il tempo di gioco
    }
}
