using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetHealthTreshold : Action
{
	public SharedInt healthTreshold;
    [SerializeField] private int howMuchToReduce = 5;
    public override void OnStart()
    {
        healthTreshold.Value = healthTreshold.Value - howMuchToReduce;
    }
}