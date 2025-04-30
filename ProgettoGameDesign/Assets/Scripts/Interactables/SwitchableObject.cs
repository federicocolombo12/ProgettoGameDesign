using DG.Tweening;
using UnityEngine;

public class SwitchableObject : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f;
    bool isOpen = false;
    public void Interact()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
        isOpen = !isOpen;
    }
    
    public void Open()
    {
        transform.DOMoveY(moveDistance, 1f).SetEase(Ease.OutBounce);
    }
    public void Close() { 
        transform.DOMoveY(0, 1f).SetEase(Ease.OutBounce);
    }
}
