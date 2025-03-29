using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float hitForce;
    [SerializeField] int speed;
    [SerializeField] float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        transform.position += speed * transform.right;
    }
    //detected hit
    private void OnTriggerEnter2D(Collider2D _other)
    {
        Debug.Log("Hit");
        if (_other.GetComponent<Enemy>() != null)
        {
            Debug.Log("Enemy Hit");
            _other.GetComponent<Enemy>().EnemyHit(damage, (_other.transform.position).normalized, -hitForce);
        }
    }

}
