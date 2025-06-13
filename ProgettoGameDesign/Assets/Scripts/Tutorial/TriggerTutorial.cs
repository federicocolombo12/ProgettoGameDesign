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
        Destroy(gameObject);
        if (tutorial.unlocked) return; // Check if the tutorial is unlocked
        tutorialEvents.RaiseEvent(tutorial);
        
    }
}
