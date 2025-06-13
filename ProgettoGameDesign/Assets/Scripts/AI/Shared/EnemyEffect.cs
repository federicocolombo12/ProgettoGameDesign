using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyEffect : EnemyAction
{
	[SerializeField] private ParticleSystem effectPrefab;
	[SerializeField] private float effectDuration = 2f;
	[SerializeField] private Vector2 effectOffset = Vector2.zero;
	[SerializeField] bool loop;

	

	public override TaskStatus OnUpdate()
	{
		if (loop)
		{
			EffectManager.Instance.PlayLooped(effectPrefab, transform.position + (Vector3)effectOffset, effectDuration, transform);
		}
		else
		{
			EffectManager.Instance.PlayOneShot(effectPrefab, transform.position + (Vector3)effectOffset);
		}
		
		
		return TaskStatus.Success;
	}
}