using BehaviorDesigner.Runtime;
using DG.Tweening;
using System;
using UnityEngine;

public class MinibossManager : MonoBehaviour
{
    [SerializeField] SwitchableObject bossFightDoor;
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] CanvasGroup bossHealthCanvas;
    BehaviorTree bossBt;
    
    private void OnEnable()
    {
        StartBossfight.OnBossfightStart += OnBossFightStart;
        Miniboss.OnEnemyDeath += OnMinibossDeath;
    }
    private void Start()
    {
        bossBt = GetComponentInChildren<BehaviorTree>();
        bossBt.DisableBehavior();
        bossHealthCanvas.alpha = 0;
    }

    private void OnBossFightStart()
    {
        bossFightDoor.Open();
        AudioManager.Instance.PlayMusic(bossFightMusic, 1f);
        bossBt.EnableBehavior();
        bossHealthCanvas.DOFade(1, 0.5f).SetEase(Ease.OutSine);
    }
    private void OnMinibossDeath()
    {
        bossFightDoor.Close();
        
    }

    
}
