using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Interactable : MonoBehaviour, IInteractable
{
    private float detectionTimeout = 0.5f; // Tempo massimo prima di nascondere la UI
    private float timeSinceLastDetection = 0f; // Timer interno

    protected bool isVisible = false;
    [SerializeField] protected CanvasGroup interactableUi;
    [SerializeField] protected ParticleSystem interactionEffect;
    [SerializeField] PlayerTransformation.AbilityType requiredAbility = PlayerTransformation.AbilityType.None;

    public virtual void Start()
    {
        interactableUi = GetComponentInChildren<CanvasGroup>();
        interactableUi.DOFade(0, 0f);
        interactionEffect = GetComponentInChildren<ParticleSystem>();


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
                        interactionEffect?.Stop();
                        interactionEffect?.Clear();
                    });

                    isVisible = false;
                }
            }
        }
    }

    public virtual void Detected(GameObject interactor)
    {
        if (!Player.Instance.playerTransformation.abilities.Contains(requiredAbility))
        return;
        if (interactableUi == null)
                return;

        // Reset il timer
        timeSinceLastDetection = 0f;

        if (!isVisible)
        {
            interactableUi.gameObject.SetActive(true);
            interactionEffect?.Play();
            interactableUi.DOFade(1, 0.2f);
            isVisible = true;
        }
    }

    public virtual void Interact(GameObject interactor)
    {
        // Override nei figli
        if (!Player.Instance.playerTransformation.abilities.Contains(requiredAbility))
        {
            Debug.Log("Non hai la trasformazione corretta per interagire con questo oggetto!");
            return;
        }
        interactableUi.DOFade(0, 0.2f).OnComplete(() =>
        {
            interactableUi.gameObject.SetActive(false);
        });
    }
}
