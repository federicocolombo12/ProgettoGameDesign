using UnityEngine;

public class BreakableRock : MonoBehaviour, IInteractable
{
    SpriteRenderer sr;
    public float interactionDistance = 2f; // distanza massima per l'interazione
    private bool isDetected = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }

    void Update()
    {
        if (isDetected)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.white;
        }

        isDetected = false; // resettiamo ogni frame, verrà riattivato da Detected se applicabile
    }

    public void Interact(GameObject interactor)
    {
        PlayerBreakableRock playerBreak = interactor.GetComponent<PlayerBreakableRock>();
        if (playerBreak != null && playerBreak.IsCharging())
        {
            Destroy(gameObject);
        }

    }

    public void Detected(GameObject interactor)
    {
        float distance = Vector2.Distance(interactor.transform.position, transform.position);
        if (distance <= interactionDistance)
        {
            isDetected = true;
        }
    }
}
