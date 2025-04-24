using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Enemies.Archer
{
	public class IsInSight2D : Conditional
	{
		private Enemy enemy;

		public override void OnStart()
		{
			enemy = GetComponent<Enemy>();
		}

		public override TaskStatus OnUpdate()
		{
		

			Vector2 direction = Player.Instance.transform.position - enemy.transform.position;
			float angle = Vector2.Angle(enemy.transform.right, direction);

			if (angle <= 45f) // Adjust the cone angle as needed (e.g., 45 degrees)
			{
				RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction.normalized, direction.magnitude);
			
				Debug.DrawRay(enemy.transform.position, direction.normalized * direction.magnitude, Color.red);
				if (hit.collider.CompareTag("Player"))
				{
					return TaskStatus.Success;
				}
			}

			return TaskStatus.Running;
		}
	}
}
