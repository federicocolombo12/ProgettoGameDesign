using UnityEngine;

public class DoNotFlip : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    Vector3 initialScale;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;

    }
    void LateUpdate()
{
    
        Vector3 parentScale = rectTransform.parent.localScale;
        rectTransform.localScale = parentScale.x > 0 ? new Vector3(initialScale.x, initialScale.y, initialScale.z) 
            : new Vector3(-initialScale.x, initialScale.y, initialScale.z);
    }
}
