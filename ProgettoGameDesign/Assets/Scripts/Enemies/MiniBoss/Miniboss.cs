using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Miniboss : Enemy
{
    [SerializeField] SpriteRenderer bossSprite;
    [SerializeField] Image bossHealthImage;
    protected override void Start()
    {
        base.Start();
        sr = bossSprite;
        healthImage = bossHealthImage;

    }
    
    
}
