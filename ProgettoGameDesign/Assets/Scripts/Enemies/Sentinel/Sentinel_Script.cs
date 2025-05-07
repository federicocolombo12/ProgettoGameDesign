using UnityEngine;

public class Sentinel : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform projectileTransform;
    
    public void Shoot()
    {
        if (player == null)
        {
            Debug.LogWarning("Player non assegnato alla sentinella!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, projectileTransform.position, Quaternion.identity);
        Vector2 projectileDirection = (player.transform.position - projectileTransform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
    }
    public override void EnemyHit(float damage, Vector2 hitDirection, float _hitForce)
    {
        base.EnemyHit(damage, hitDirection, _hitForce);

        if (health <= 0)
        {
            Death(1.5f); 
        }
    }

}
