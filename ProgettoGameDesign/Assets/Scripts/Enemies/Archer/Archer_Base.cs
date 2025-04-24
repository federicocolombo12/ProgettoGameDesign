using UnityEngine;

public class Archer_Base : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed;
    private Rigidbody2D projectileRb;
    protected override void Start()
    {
        base.Start();
        projectileRb = projectilePrefab.GetComponent<Rigidbody2D>();
        
    }
    public void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 projectileDirection = (player.transform.position - transform.position).normalized;
        projectileRb.linearVelocity = projectileDirection * projectileSpeed;
    }

    
}
