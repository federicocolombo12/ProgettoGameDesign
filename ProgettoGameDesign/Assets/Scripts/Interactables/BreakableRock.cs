using UnityEngine;

public class BreakableRock : Interactable
{
    SpriteRenderer sr;
    
    
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
            
           
        }

    }
   

}
