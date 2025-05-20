using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using DG.Tweening;
public class Interactable : MonoBehaviour, IInteractable
{
    private float detectionTimeout = 0.5f; // tempo massimo senza detection prima di nascondere la UI
    private float lastDetectionTime;

    private bool isVisible = false;
    [SerializeField] protected CanvasGroup interactableUi;
    public virtual void Start()
    {
        
        
        interactableUi = GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (isVisible && Time.time - lastDetectionTime > detectionTimeout)
        {
           if (interactableUi != null)
            {
               interactableUi.DOFade(0, 0.2f).OnComplete(() =>
            {
                interactableUi.gameObject.SetActive(false);
            });
            isVisible = false;
            }
            
        }
    }

    public virtual void Detected(GameObject interactor)
    {
        if (interactableUi == null)
        {
            return;
        }
        lastDetectionTime = Time.time;
        
        if (!isVisible)
        {
            interactableUi.gameObject.SetActive(true);
            interactableUi.DOFade(1, 0.2f);
            isVisible = true;
        }
    }

    public virtual void Interact(GameObject interactor)
    {
       
    }
    
}
