using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Vector2 activationMin;
    public Vector2 activationMax;

    private ParallaxScript[] parallaxLayers;
    private Transform cameraTransform;

    void Start()
    {
        parallaxLayers = GetComponentsInChildren<ParallaxScript>(true);
        cameraTransform = Camera.main.transform;

        BoxCollider2D zone = GetComponent<BoxCollider2D>();
        if (zone != null)
        {
            Vector2 size = zone.size;
            Vector2 center = (Vector2)transform.position + zone.offset;

            activationMin = center - size * 0.5f;
            activationMax = center + size * 0.5f;
        }
    }

    void Update()
    {
        bool isActive = cameraTransform.position.x >= activationMin.x &&
                        cameraTransform.position.x <= activationMax.x &&
                        cameraTransform.position.y >= activationMin.y &&
                        cameraTransform.position.y <= activationMax.y;

        foreach (var layer in parallaxLayers)
        {
            layer.enabled = isActive;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = (activationMin + activationMax) * 0.5f;
        Vector3 size = activationMax - activationMin;
        Gizmos.DrawWireCube(center, size);
    }
}
