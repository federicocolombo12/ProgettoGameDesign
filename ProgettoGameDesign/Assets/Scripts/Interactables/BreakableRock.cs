using UnityEngine;

public class BreakableRock : MonoBehaviour, IInteractable
{
    SpriteRenderer sr;
    [SerializeField] GameObject rockEffect;
    public void Start()
    {


        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }
    public void Update()
    {
        

        if (sr.color == Color.red)
        {
            sr.color = Color.white;
        }
    }
    public void Interact(GameObject interactor)
    {
        if (interactor.GetComponent<PlayerBreakableRock>() != null)
        {

            interactor.GetComponent<PlayerBreakableRock>().ChargeAndBreak(transform);
            EffectManager.Instance.PlayOneShot(rockEffect.GetComponent<ParticleSystem>(), transform.position);

            sr.color = Color.green;
        }

    }
    public void Detected(GameObject interactor)
    {

        if (interactor.GetComponent<PlayerBreakableRock>() != null)
        {
            sr.color = Color.red;
        }

    }

}
