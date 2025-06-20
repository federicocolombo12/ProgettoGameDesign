using UnityEngine;
using DG.Tweening;

public class DoMoveLoop : MonoBehaviour
{
    [SerializeField] float moveDistancey = 10f; // Distance to move
    [SerializeField] float moveDistancex = 10f; // Distance to move
    [SerializeField] float moveDuration = 5f; // Duration of the move
    void Start()
    {
        //move this rect transform looping to animate them in the background
        RectTransform rectTransform = GetComponent<RectTransform>();
        //using DOTween for smooth looping animation
        if (rectTransform != null) {
            
            rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x-moveDistancex,rectTransform.anchoredPosition.y
                -moveDistancey), moveDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        } else {
            Debug.LogWarning("RectTransform component not found on this GameObject.");
        }
    }

    // Update is called once per frame
    
}
