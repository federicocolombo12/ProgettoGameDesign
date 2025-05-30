using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour, IInteractable
{
    private float detectionTimeout = 0.5f; // Tempo massimo prima di nascondere la UI
    private float timeSinceLastDetection = 0f; // Timer interno

    protected bool isVisible = false;
    [SerializeField] protected CanvasGroup interactableUi;

    public virtual void Start()
    {
        interactableUi = GetComponentInChildren<CanvasGroup>();
        interactableUi.DOFade(0, 0f); // Inizialmente invisibile
    }

    public virtual void Update()
    {
        if (isVisible)
        {
            timeSinceLastDetection += Time.deltaTime;

            if (timeSinceLastDetection > detectionTimeout)
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
    }

    public virtual void Detected(GameObject interactor)
    {
        if (interactableUi == null)
            return;

        // Reset il timer
        timeSinceLastDetection = 0f;

        if (!isVisible)
        {
            interactableUi.gameObject.SetActive(true);
            interactableUi.DOFade(1, 0.2f);
            isVisible = true;
        }
    }

    public virtual void Interact(GameObject interactor)
    {
        // Override nei figli
        interactableUi.DOFade(0, 0.2f).OnComplete(() =>
        {
            interactableUi.gameObject.SetActive(false);
        });
    }
}
