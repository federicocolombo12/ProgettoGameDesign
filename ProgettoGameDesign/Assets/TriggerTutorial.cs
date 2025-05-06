using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    [SerializeField] Tutorial tutorial;
    [SerializeField] TutorialEvents tutorialEvents;
    Collider2D triggerCollider;
    private void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        triggerCollider.isTrigger = true;
    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialEvents.RaiseEvent(tutorial);
        Destroy(gameObject);
    }
}
