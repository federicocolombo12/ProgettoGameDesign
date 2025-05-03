using UnityEngine;

public class Sentinel : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform projectileTransform;
    public Transform centerOfInfluence;

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
}
