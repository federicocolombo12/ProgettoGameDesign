using Dev.Nicklaj.Butter;
using DG.Tweening;
using UnityEngine;

public class Bench : Interactable
{
    
    [SerializeField] public int benchIndex;
    [SerializeField] IntVariable benchIndexVariable;
    public string benchSceneName;
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        Debug.Log("Interacting with bench");
        benchIndexVariable.Value = benchIndex;
        GameManager.Instance.SetBench(this);

    }
}
