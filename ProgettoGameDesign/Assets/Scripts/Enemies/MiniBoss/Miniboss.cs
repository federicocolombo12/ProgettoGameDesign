using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Miniboss : Enemy
{
    [SerializeField] SpriteRenderer bossSprite;
    [SerializeField] Image bossHealthImage;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
        animator = GetComponentInChildren<Animator>();
        
        
        
        player = Player.Instance;
        healthImage = GetComponentInChildren<Image>();
        health = maxHealth;
        sr = bossSprite;
        standardMaterial = sr.material;
        healthImage = bossHealthImage;

    }

    
    
}
