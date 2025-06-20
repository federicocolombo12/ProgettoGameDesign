using DG.Tweening;
using UnityEngine;

public class DoZoomLoop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float zoomInScale = 0.14f; // Scale to zoom in
    [SerializeField] float zoomOutScale = 0.12f; // Scale to zoom out
    [SerializeField] float duration = 1f; // Duration of the zoom in and out
    void Start()
    {
        //zoom in and out continuously this image tweening
        //using DOTween for tweening
        transform.localScale = Vector3.one * zoomOutScale; // Start with a smaller scale
        transform.localScale = Vector3.one * zoomInScale; // Start with a larger scale
        transform.localScale = Vector3.one * 0.13f; // Reset to normal scale
        transform.DOScale(Vector3.one * zoomInScale, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
