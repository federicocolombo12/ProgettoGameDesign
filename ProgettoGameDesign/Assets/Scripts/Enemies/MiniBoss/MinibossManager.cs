using BehaviorDesigner.Runtime;
using DG.Tweening;
using System;
using UnityEngine;

public class MinibossManager : MonoBehaviour
{
    [SerializeField] SwitchableObject bossFightDoor; // The door that opens when the boss fight starts
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] CanvasGroup bossHealthCanvas;
    BehaviorTree bossBt;

    private void OnEnable()
    {
        StartBossfight.OnBossfightStart += OnBossFightStart;
        
        
    }
    private void OnDisable()
    {
        StartBossfight.OnBossfightStart -= OnBossFightStart;
        
        
    }
    private void Start()
    {
        bossBt = GetComponentInChildren<BehaviorTree>();
        bossBt.DisableBehavior();
        bossHealthCanvas.alpha = 0;
        
    }

    private void OnBossFightStart()
    {
        if (bossFightMusic != null) {
            AudioManager.Instance.PlayMusic(bossFightMusic, 1f);
        }
        
        bossBt.EnableBehavior();
        bossHealthCanvas.DOFade(1, 0.5f).SetEase(Ease.OutSine);
    }
    private void OnMinibossDeath()
    {
       bossFightDoor = FindFirstObjectByType<SwitchableObject>();
        bossFightDoor.Open();
        
    }

    
}
