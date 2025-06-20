using BehaviorDesigner.Runtime;
using DG.Tweening;
using System;
using UnityEngine;

public class MinibossManager : MonoBehaviour
{
    [SerializeField] SwitchableObject bossFightDoor; // The door that opens when the boss fight starts
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] CanvasGroup bossHealthCanvas;
    [SerializeField] AudioClip postFightMusic;

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
        bossFightDoor.Open();
    }
    private void OnMinibossDeath()
    {
       bossFightDoor = FindFirstObjectByType<SwitchableObject>();
        bossFightDoor.Close();
        AudioManager.Instance.PlayMusic(postFightMusic, 1f);
        bossHealthCanvas.DOFade(0, 0.5f).SetEase(Ease.InSine);
        
    }

    
}
