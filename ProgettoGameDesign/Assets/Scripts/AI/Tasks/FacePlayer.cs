using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FacePlayer : EnemyAction
{
	private float baseScaleX;
	public override void OnStart()
	{
		base.OnStart();
		baseScaleX = transform.localScale.x;
	}

	public override TaskStatus OnUpdate()
	{
		// Get the player object
		var scale = transform.localScale;
		scale.x = transform.position.x > player.transform.position.x ? 1 : -1;
		transform.localScale = scale;
		return TaskStatus.Success;
	}
}