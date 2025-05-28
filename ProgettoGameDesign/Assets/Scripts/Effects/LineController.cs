using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] Texture2D[] lineTextures;
    private int animationSteps;
    [SerializeField] private float fps = 30f;
    private float fpsCounter = 0f;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            animationSteps++;
            if (animationSteps >= lineTextures.Length)
            {
                animationSteps = 0;
            }
           
            
            lineRenderer.material.SetTexture("_MainTex", lineTextures[animationSteps]);
             fpsCounter = 0f;
        }
    }
}
