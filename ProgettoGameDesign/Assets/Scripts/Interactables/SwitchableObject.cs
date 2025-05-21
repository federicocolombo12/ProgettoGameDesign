using System;
using DG.Tweening;
using UnityEngine;

public class SwitchableObject : Interactable
{
    [SerializeField] private float moveDistance = 1f;
    Vector2 startPos;
    bool isOpen = false;
    

    public override void Start()
    {
        base.Start();
        startPos = transform.position;
    
    }

   
    

    public override void Interact(GameObject interactor)
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
