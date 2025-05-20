using DG.Tweening;
using UnityEngine;

public class Bench : Interactable
{
    

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        Debug.Log("Interacting with bench");
        GameManager.Instance.SetBench(this);
    }
}
