using UnityEngine;

public class BreakableRock : Interactable
{
    SpriteRenderer sr;
    [SerializeField] GameObject rockEffect;
    public override void Start()
    {

        base.Start();
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }
    
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);
        if (interactor.GetComponent<PlayerBreakableRock>() != null)
        {

            interactor.GetComponent<PlayerBreakableRock>().ChargeAndBreak(transform);
            EffectManager.Instance.PlayOneShot(rockEffect.GetComponent<ParticleSystem>(), transform.position);

            sr.color = Color.green;
        }

    }
   

}
