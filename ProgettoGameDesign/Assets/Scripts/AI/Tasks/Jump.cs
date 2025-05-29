using BehaviorDesigner.Runtime.Tasks;

using DG.Tweening;
using TMPro.Examples;
using UnityEngine;



    public class Jump : EnemyAction
    {
        public float horizontalForce = 5.0f;
        public float jumpForce = 10.0f;

        public float buildupTime;
        public float jumpTime;

        public string triggerStart;
        public string animationBase;
        public string animationEnd;
        public bool shakeCameraOnLanding;

        private bool hasLanded;

        private Tween buildupTween;
        private Tween jumpTween;

        public SpriteRenderer jumpEffect;
        public Vector3 effectOffset;


        public override void OnStart()
        {
            buildupTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
            anim.SetTrigger(triggerStart);
        }

        private void StartJump()
        {
            if (animationBase != "")
            {
                anim.SetTrigger(animationBase);
            }
            var direction = player.transform.position.x < transform.position.x ? -1 : 1;
            rb.AddForce(new Vector2(horizontalForce * direction, jumpForce), ForceMode2D.Impulse);
            if (jumpEffect != null)
            {
                EffectManager.Instance.PlaySpriteOneShot(jumpEffect, transform.position+ effectOffset, direction > 0);
            }
            jumpTween = DOVirtual.DelayedCall(jumpTime, () =>
            {
                rb.linearVelocity = Vector2.zero;
                hasLanded = true;
                if (animationEnd != "")
                {
                    anim.SetTrigger(animationEnd);
                }
                if (shakeCameraOnLanding)
                    CameraManager.Instance.ShakeCamera(0.5f);
            }, false);

        }

        public override TaskStatus OnUpdate()
        {
            return hasLanded ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            buildupTween?.Kill();
            jumpTween?.Kill();
            hasLanded = false;
        }
    }
