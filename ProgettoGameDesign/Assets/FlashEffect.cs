using UnityEngine;
using DG.Tweening;

public class FlashEffect : MonoBehaviour
{


    public AnimationCurve flashCurve;

    private Material mat;
    private float dissolveAmount = 1f;
    private Tween currentTween;
    float oscillationSpeed = 3f; // Speed of the oscillation effect

    void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_DissolveAmount", dissolveAmount);
    }
    private void OnEnable()
    {
        PlayerHealth.OnPlayerHit += FlashDissolve;
        PlayerTransform.OnTransformEffect += FlashDissolve;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerHit -= FlashDissolve;
        PlayerTransform.OnTransformEffect -= FlashDissolve;
    }

    public void FlashDissolve(float flashDuration, Color color)
    {
        if (currentTween != null && currentTween.IsPlaying())
            currentTween.Kill();

        mat.SetColor("_DissolveColor", color);

        // Calcola la durata di un ciclo (1→0→1)
        float singleCycleDuration = 1f / oscillationSpeed;

        // Ogni ciclo è composto da 2 tween (1→0, 0→1), quindi loopCount è flashDuration diviso metà ciclo
        int loopCount = Mathf.FloorToInt(flashDuration / (singleCycleDuration));

        currentTween = DOTween.To(() => dissolveAmount, x =>
        {
            dissolveAmount = x;
            mat.SetFloat("_DissolveAmount", dissolveAmount);
        }, 0f, singleCycleDuration / 2f) // durata metà ciclo
        .SetEase(flashCurve)
        .SetLoops(loopCount, LoopType.Yoyo)
        .OnKill(() =>
        {
            // Alla fine, riporta dissolveAmount a 1 (opzionale)
            dissolveAmount = 1f;
            mat.SetFloat("_DissolveAmount", dissolveAmount);
        });

    }
}
