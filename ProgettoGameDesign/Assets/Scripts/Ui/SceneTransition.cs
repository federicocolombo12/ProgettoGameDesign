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
            Player.Instance.transform.position = startPoint.position;
            StartCoroutine(Player.Instance.playerMovement.WalkIntoNewScene(exitDirection, exitTime));
            
        }
        if (UiManager.Instance == null)
        {
            Debug.LogError(" not found in UiManager");
        }
        StartCoroutine(UiManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Start the transition to the new scene, setting transitionedFromScene to the scene the object is currently in
            GameManager.Instance.transitionedFromScene = GameManager.Instance.GetGameplaySceneName();
            Player.Instance.pState.cutscene = true;
            StartCoroutine(UiManager.Instance.sceneFader.FadeAndLoadScene(SceneFader.FadeDirection.In, transitionTo));
        }
    }
}
