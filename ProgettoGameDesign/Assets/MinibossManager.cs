using BehaviorDesigner.Runtime;
using System;
using UnityEngine;

public class MinibossManager : MonoBehaviour
{
    [SerializeField] SwitchableObject bossFightDoor;
    [SerializeField] AudioClip bossFightMusic;
    BehaviorTree bossBt;
    
    private void OnEnable()
    {
        StartBossfight.OnBossfightStart += OnBossFightStart;
        Miniboss.OnEnemyDeath += OnMinibossDeath;
    }
    private void Start()
    {
        bossBt = GetComponentInChildren<BehaviorTree>();
    }

    private void OnBossFightStart()
    {
        bossFightDoor.Open();
        AudioManager.Instance.PlayMusic(bossFightMusic, 1f);
        bossBt.EnableBehavior();
    }
    private void OnMinibossDeath()
    {
        bossFightDoor.Close();
        
    }

    
}
