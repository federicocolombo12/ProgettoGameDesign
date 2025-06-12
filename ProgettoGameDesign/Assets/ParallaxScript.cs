using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    [Header("Fattore di parallasse ")]
    public float parallaxFactor = 0.5f;

    [Header("Assi abilitati")]
    public bool enableHorizontal = true;
    public bool enableVertical = false;

    private Vector3 lastCameraPosition;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam != null)
            lastCameraPosition = cam.transform.position;
    }

    void Update()
    {
        if (cam == null) return;

        Vector3 delta = cam.transform.position - lastCameraPosition;

        float deltaX = enableHorizontal ? delta.x * parallaxFactor : 0f;
        float deltaY = enableVertical ? delta.y * parallaxFactor : 0f;

        transform.position += new Vector3(deltaX, deltaY, 0);
        lastCameraPosition = cam.transform.position;
    }
}
