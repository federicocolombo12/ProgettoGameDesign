using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string transitionTo;
    [SerializeField] Transform startPoint;
    [SerializeField] private Vector2 exitDirection;
    [SerializeField] private float exitTime = 1f;
    private void Start()
    {
        if (transitionTo == GameManager.Instance.transitionedFromScene)
        {
            // Set the player's position to the start point
           PlayerController.Instance.transform.position = startPoint.position;
            StartCoroutine(PlayerController.Instance.WalkIntoNewScene(exitDirection,exitTime));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Start the transition to the new scene
            GameManager.Instance.transitionedFromScene = SceneManager.GetActiveScene().name;
            PlayerController.Instance.pState.cutscene = true;
            SceneManager.LoadScene(transitionTo);
        }
    }
}
