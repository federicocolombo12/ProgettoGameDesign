using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ShootFire : Action
{
    private Sentinel sentinel;

    public override void OnStart()
    {
        sentinel = GetComponent<Sentinel>();

        if (sentinel == null)
        {
            Debug.LogError(" ShootProjectile: componente Sentinel non trovato!");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (sentinel == null)
        {
            return TaskStatus.Failure;
        }

        sentinel.Shoot();
        return TaskStatus.Success;
    }
}
