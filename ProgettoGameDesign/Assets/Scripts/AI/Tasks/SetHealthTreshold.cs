using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetHealthTreshold : Action
{
	public SharedInt healthTreshold;
    public override void OnStart()
    {
        healthTreshold.Value = healthTreshold.Value - 5;
    }
}