using UnityEngine;

public class Archer_Base : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed;
    [SerializeField] Transform projectileTransform;
    public Transform centerOfInfluence;
    
    
    
    public void Shoot()
    {
        

        GameObject projectile=Instantiate(projectilePrefab, projectileTransform.position, Quaternion.identity);
        Vector2 projectileDirection = (Player.Instance.transform.position - projectileTransform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
    }

    
}
