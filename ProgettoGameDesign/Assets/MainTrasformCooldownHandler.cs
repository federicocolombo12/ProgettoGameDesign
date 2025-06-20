using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainTrasformCooldownHandler : MonoBehaviour
{
    Image mainImage;
    private void Start()
    {
        mainImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        PlayerTransform.OnTransformEffect += HandleTransformCooldown;
    }
    private void OnDisable()
    {
        PlayerTransform.OnTransformEffect -= HandleTransformCooldown;
    }

    private void HandleTransformCooldown(float timer, Color color)
    {
        mainImage.fillAmount = 0f;
        //lerp the value with do tween to 1
        mainImage.DOFillAmount(1f, timer).SetEase(Ease.Linear);
    }
}
