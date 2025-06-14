using BehaviorDesigner.Runtime.Tasks;

using DG.Tweening;
using TMPro.Examples;
using UnityEngine;



    public class DestroyBoss : EnemyAction
    {
        public float bleedTime = 2.0f;
        public ParticleSystem bleedEffect;
        public ParticleSystem explodeEffect;
    [SerializeField] private SwitchableObject switchableObject;

        private bool isDestroyed;

        public override void OnStart()
        {
            EffectManager.Instance.PlayOneShot(bleedEffect, transform.position);
            DOVirtual.DelayedCall(bleedTime, () =>
            {
                EffectManager.Instance.PlayOneShot(explodeEffect, transform.position);
                switchableObject?.Open();
                CameraManager.Instance.ShakeCamera(0.7f);
                isDestroyed = true;
                Object.Destroy(gameObject);
            }, false);
        }

        public override TaskStatus OnUpdate()
        {
            return isDestroyed ? TaskStatus.Success : TaskStatus.Running;
        }
    }
