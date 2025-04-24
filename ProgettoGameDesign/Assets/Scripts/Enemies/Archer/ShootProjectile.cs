using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ShootProjectile : Action
{
	Archer_Base archer;
	public override void OnStart()
	{
		archer = GetComponent<Archer_Base>();
	}

	public override TaskStatus OnUpdate()
	{
		archer.Shoot();
		return TaskStatus.Success;
	}
}