using System;
using DG.Tweening;
using UnityEngine;

public class SwitchableObject : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f;
    Vector2 startPos;
    bool isOpen = false;

    private void Start()
    {
        startPos = transform.position;
    
    }

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
        transform.DOMoveY(startPos.y, 1f).SetEase(Ease.OutBounce);
    }
}
