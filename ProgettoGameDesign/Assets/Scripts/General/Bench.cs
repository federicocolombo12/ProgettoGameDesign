using DG.Tweening;
using UnityEngine;

public class Bench : Interactable
{
    private float detectionTimeout = 0.5f; // tempo massimo senza detection prima di nascondere la UI
    private float lastDetectionTime;

    private bool isVisible = false;

    private void Update()
    {
        if (isVisible && Time.time - lastDetectionTime > detectionTimeout)
        {
            // Nascondi se è passato troppo tempo dall'ultima Detected()
            interactableUi.DOFade(0, 0.2f).OnComplete(() =>
            {
                interactableUi.gameObject.SetActive(false);
            });
            isVisible = false;
        }
    }

    public override void Detected(GameObject interactor)
    {
        base.Detected(interactor);
        lastDetectionTime = Time.time;
        Debug.Log("Detected bench");
        if (!isVisible)
        {
            interactableUi.gameObject.SetActive(true);
            interactableUi.DOFade(1, 0.2f);
            isVisible = true;
        }
    }

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        Debug.Log("Interacting with bench");
        GameManager.Instance.SetBench(this);
    }
}
