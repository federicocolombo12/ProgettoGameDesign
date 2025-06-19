using Dev.Nicklaj.Butter;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class Bench : Interactable
{
    
    [SerializeField] public int benchIndex;
    [SerializeField] IntVariable benchIndexVariable;
    [SerializeField] GameObject interactEffect;
    public int benchCamera;
    public string benchSceneName;
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        Debug.Log("Interacting with bench: " + benchIndexVariable);
        benchIndexVariable.Value = benchIndex;
        GameManager.Instance.SetBench();
        CameraManager.Instance.SetCamera(benchCamera);
       
        EffectManager.Instance.PlayOneShot(interactEffect.GetComponent<ParticleSystem>(), transform.position);


    }
}
