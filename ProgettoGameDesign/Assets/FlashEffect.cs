using UnityEngine;
using DG.Tweening;

public class FlashEffect : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color flashColor = Color.white;
    
    public AnimationCurve flashCurve;

    private Material mat;
    private float dissolveAmount = 1f;
    private Tween currentTween;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_DissolveAmount", dissolveAmount);
    }
    private void OnEnable()
    {
        PlayerHealth.OnPlayerHit += FlashDissolve;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerHit -= FlashDissolve;
    }

    public void FlashDissolve(float flashDuration, Color color)
    {
        // Stop any ongoing tween
        if (currentTween != null && currentTween.IsPlaying())
            currentTween.Kill();

        // Set color
        mat.SetColor("_DissolveColor", flashColor);

        // Animazione dissolveAmount da 1 → 0 → 1 seguendo la curva
        currentTween = DOTween.To(() => dissolveAmount, x =>
        {
            dissolveAmount = x;
            mat.SetFloat("_DissolveAmount", dissolveAmount);
        }, 0f, flashDuration / 2f)
        .SetEase(flashCurve)
        .OnComplete(() =>
        {
            currentTween = DOTween.To(() => dissolveAmount, x =>
            {
                dissolveAmount = x;
                mat.SetFloat("_DissolveAmount", dissolveAmount);
            }, 1f, flashDuration / 2f)
            .SetEase(flashCurve);
        });
    }
}
