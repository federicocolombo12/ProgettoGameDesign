using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    public float parallaxFactor = 0.5f;
    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = Camera.main.transform.position;
    }

    void Update()
    {
        Vector3 delta = Camera.main.transform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
        lastCameraPosition = Camera.main.transform.position;
    }
}
