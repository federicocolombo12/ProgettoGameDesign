using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.AI
{
    public class IsHealthUnderTreshold : EnemyConditional
    {
        public SharedInt HealthTreshold;

        public override TaskStatus OnUpdate()
        {
            return enemy.health < HealthTreshold.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}