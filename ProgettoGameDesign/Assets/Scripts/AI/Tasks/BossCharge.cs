using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DG.Tweening;

public class BossCharge : EnemyAction
{
    public float chargeSpeed = 10f;
    public float maxChargeTime = 2f; // timeout di sicurezza
    public string chargeStartAnim;
    public string chargeLoopAnim;
    public string chargeEndAnim;
    public bool shakeCameraOnImpact;

    private bool hasHitWall;
    private float chargeTimer;
    private int direction;
    private Tween timeoutTween;

    public override void OnStart()
    {
        hasHitWall = false;
        chargeTimer = 0f;

        if (!string.IsNullOrEmpty(chargeStartAnim))
            anim.SetTrigger(chargeStartAnim);

        direction = player.transform.position.x < transform.position.x ? -1 : 1;

        if (!string.IsNullOrEmpty(chargeLoopAnim))
            anim.SetTrigger(chargeLoopAnim);
        timeoutTween = DOVirtual.DelayedCall(maxChargeTime, () =>
        {
            StopCharge();
        }, false);

        
    }

    public override TaskStatus OnUpdate()
    {
        if (hasHitWall) return TaskStatus.Success;

        // Movimento continuo
        rb.linearVelocity = new Vector2(direction * chargeSpeed, rb.linearVelocity.y);

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        StopCharge();
    }

    private void StopCharge()
    {
        rb.linearVelocity = Vector2.zero;
        hasHitWall = true;

        if (!string.IsNullOrEmpty(chargeEndAnim))
            anim.SetTrigger(chargeEndAnim);

        if (shakeCameraOnImpact)
            CameraManager.Instance.ShakeCamera(0.4f);

        timeoutTween?.Kill();
    }

    // Collisione con muri
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (hasHitWall) return;

        // Puoi cambiare in base al tuo setup
        //usa il layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StopCharge();
        }
    }
}
