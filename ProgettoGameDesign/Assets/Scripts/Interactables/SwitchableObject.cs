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
        startPos = transform.localPosition; // usa localPosition

        if (doorOpen.isOpen)
        {
            isOpen = true;
            transform.localPosition = new Vector3(startPos.x, moveDistance,0);
        }
        else
        {
            isOpen = false;
            transform.localPosition = new Vector3(startPos.x, startPos.y, 0);
        }
    }

    public void Open()
    {
        transform.DOLocalMoveY(moveDistance, 1f).SetEase(Ease.OutBounce);
    }

    public void Close()
    {
        transform.DOLocalMoveY(startPos.y, 1f).SetEase(Ease.OutBounce);
    }

}
