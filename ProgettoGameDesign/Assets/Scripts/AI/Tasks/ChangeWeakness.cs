using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using DG.Tweening;
using System.Collections.Generic;


public class ChangeWeakness : EnemyAction
{
	private int weakness;
	[SerializeField] Color[] weaknessColors;
	
	[SerializeField] List<PlayerTransformation> playerTransformations; // List of possible transformations
	[SerializeField] Image healthBarWeaknessImage;
	[SerializeField] float attackDuration = 1f;
	[SerializeField] GameObject fireAura;
	public override void OnStart()
	{
		weakness = Random.Range(0, 3);
		switch (weakness)
		{
			case 0:
				healthBarWeaknessImage.color = weaknessColors[0];
				enemy.sr.color = weaknessColors[0];
				break;
			case 1:
				healthBarWeaknessImage.color = weaknessColors[1];
				enemy.sr.color = weaknessColors[1];
				break;
			case 2:
				healthBarWeaknessImage.color = weaknessColors[2];
				enemy.sr.color = weaknessColors[2];
				break;
			default:
				Debug.LogError("Invalid weakness value: " + weakness);
				break;
		}
	}

	public override TaskStatus OnUpdate()
	{
		fireAura.SetActive(true);
		fireAura.GetComponent<ParticleSystem>().Play();
		enemy.weakTo = playerTransformations[weakness];
		DOVirtual.DelayedCall(attackDuration, () =>
		{
			fireAura.GetComponent<ParticleSystem>().Stop();
			fireAura.GetComponent<ParticleSystem>().Clear();
			fireAura.SetActive(false);
			
		});
		return TaskStatus.Success;
	}
}