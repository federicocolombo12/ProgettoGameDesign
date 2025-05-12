using UnityEngine;

public class DoNotFlip : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void LateUpdate()
{
    Vector3 parentScale = transform.parent.localScale;
    
    rectTransform.localScale = parentScale.x > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
}
}
