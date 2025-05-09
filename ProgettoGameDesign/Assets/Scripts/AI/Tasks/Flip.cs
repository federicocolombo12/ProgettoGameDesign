using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Flip : EnemyAction
{

	
	public BehaviorTree behavior;
	public override TaskStatus OnUpdate()
	{
		Vector2 target = ((SharedVector2)behavior.GetVariable("targetPosition")).Value;
        transform.localScale = target.x > transform.position.x
            ? new Vector3(-1, 1, 1)
            : new Vector3(1, 1, 1);
        return TaskStatus.Success;
	}
}