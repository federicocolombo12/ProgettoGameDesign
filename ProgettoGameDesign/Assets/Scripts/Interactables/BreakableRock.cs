using UnityEngine;

public class BreakableRock : MonoBehaviour, IInteractable
{
    private SpriteRenderer sr;

    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color detectedColor = Color.red;
    [SerializeField] private Color interactedColor = Color.green;

    private bool isDetected = false;
    private bool isInteracted = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    public void Detected(GameObject interactor)
    {
        if (interactor.GetComponent<PlayerBreakableRock>() != null && !isInteracted)
        {
            isDetected = true;
            sr.color = detectedColor;
        }
    }

    public void Interact(GameObject interactor)
    {
        if (interactor.GetComponent<PlayerBreakableRock>() != null && !isInteracted)
        {
            isInteracted = true;
            sr.color = interactedColor;

            // Qui viene chiamata la carica dal player
            interactor.GetComponent<PlayerBreakableRock>().ChargeAndBreak(transform);
        }
    }

    private void Update()
    {
        if (!isDetected && !isInteracted)
        {
            sr.color = normalColor;
        }

        isDetected = false; // Ogni frame resetto il detect, verrà reimpostato se rilevato di nuovo
    }
}
