using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

public class DoMove : EnemyAction
{
	public BehaviorTree behavior; // Assicurati che sia assegnato

    [SerializeField] Collider2D groundLength;
    [SerializeField] float duration;

    public override TaskStatus OnUpdate()
    {
        var target = new Vector2(Random.Range(groundLength.bounds.min.x, groundLength.bounds.max.x), transform.position.y);

        // Ottieni la variabile condivisa dal BehaviorTree e aggiornala
        var sharedTarget = (SharedVector2)behavior.GetVariable("targetPosition");
        sharedTarget.Value = target;

        // Muovi
        rb.DOMoveX(target.x, duration)
            .SetSpeedBased()
            .SetEase(Ease.Linear).
            OnComplete(() =>
            {
                // Al termine del movimento, aggiorna la posizione del nemico
                anim.SetTrigger("Idle");
            });

        return TaskStatus.Success;
    }
}