using System;
using DG.Tweening;
using UnityEngine;

public class SwitchableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private float moveDistance = 1f;
    Vector2 startPos;
    bool isOpen = false;
    

    private void Start()
    {
        startPos = transform.position;
    
    }

    public void Detected(GameObject interactor)
    {
        // Optional: Add any detection logic if needed
    }
    

    public void Interact(GameObject interactor)
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
        transform.DOLocalMoveY(moveDistance, 1f).SetEase(Ease.OutBounce);
    }
    public void Close() { 
        transform.DOLocalMoveY(startPos.y, 1f).SetEase(Ease.OutBounce);
    }
}
