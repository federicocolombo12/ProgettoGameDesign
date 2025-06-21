using System;
using DG.Tweening;
using UnityEngine;

public class SwitchableObject : Interactable
{
    [SerializeField] private float moveDistance = 1f;
    Vector2 startPos;
    bool isOpen = false;
    [SerializeField] BenchInstance doorOpen;

    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        //open or close it based on the state of the doorOpen instance
        if (doorOpen.isOpen)
        {
            isOpen = true;
            transform.localPosition = new Vector3(transform.localPosition.x, moveDistance, transform.localPosition.z);
        }
        else
        {
            isOpen = false;
            transform.localPosition = new Vector3(transform.localPosition.x, startPos.y, transform.localPosition.z);
        }
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
        doorOpen.isOpen = isOpen;
    }
    
    public void Open()
    {
        transform.DOLocalMoveY(moveDistance, 1f).SetEase(Ease.OutBounce);
    }
    public void Close() { 


        transform.DOLocalMoveY(startPos.y, 1f).SetEase(Ease.OutBounce);
    }

}
