using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AudioPlay : Action
{
	[SerializeField] SfxData sfxData;
	public override void OnStart()
	{
		AudioManager.Instance.sfxChannel.RaiseEvent(sfxData, true);
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}