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
    private void OnCollisionEnter2D(Collision2D _other)
    {
        Debug.Log("Hit");
        if (_other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Hit");
            _other.gameObject.GetComponent<Enemy>().EnemyHit(damage, (_other.transform.position).normalized, -hitForce);
        }
    }

}
