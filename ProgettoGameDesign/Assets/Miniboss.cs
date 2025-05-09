using Sirenix.OdinInspector;
using UnityEngine;

public class Miniboss : Enemy
{
    [SerializeField] SpriteRenderer bossSprite;
    protected override void Start()
    {
        base.Start();
        sr = bossSprite;
    }
    [Button("Test Hit")][SerializeField]
    protected override void EnemyHitFeedback()
    {
        base.EnemyHitFeedback();

    }
}
