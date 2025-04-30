using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector2 offset = new Vector2(6f, 0f);
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float waitDuration = 2f;
    private SpriteRenderer spriteRenderer;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool movingToTarget = true;
    private bool isGreen;
    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + offset;
        StartCoroutine(MoveLoop());
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;
        isGreen = true;
        
        
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            Vector2 destination = movingToTarget ? targetPosition : startPosition;

            yield return transform.DOMove(destination, moveDuration).SetEase(Ease.Linear).WaitForCompletion();
            yield return new WaitForSeconds(waitDuration);

            movingToTarget = !movingToTarget;
        }
    }
    public void Detected(GameObject player)
    {
        // Optional: Add any logic when the player detects the platform
    }
    public void Interact(GameObject player)
    {
        // Optional: Add any logic when the player interacts with the platform
        ChangeOffset(new Vector2(offset.y, offset.x));
        isGreen = !isGreen;
        spriteRenderer.color = isGreen ? Color.green : Color.red;
    }
    void ChangeOffset(Vector2 newOffset)
    {
       
        offset = newOffset;
        targetPosition = startPosition + offset;
    }
}