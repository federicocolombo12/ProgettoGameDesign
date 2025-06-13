using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

[TaskDescription("Random Selector che non ripete lo stesso figlio due volte di fila.")]
[TaskIcon("{SkinColor}SelectorIcon.png")]
[TaskCategory("Custom/Composites")]
public class SmartRandomSelector : Composite
{
    private int lastChildIndex = -1;
    private int currentChildIndex = -1;
    private TaskStatus executionStatus = TaskStatus.Inactive;
    private List<int> childIndices = new List<int>();

    public override void OnStart()
    {
        childIndices.Clear();

        for (int i = 0; i < children.Count; i++)
        {
            if (i != lastChildIndex)
            {
                childIndices.Add(i);
            }
        }

        currentChildIndex = childIndices[Random.Range(0, childIndices.Count)];
        executionStatus = TaskStatus.Inactive;
    }

    public override int CurrentChildIndex()
    {
        return currentChildIndex;
    }

    public override bool CanExecute()
    {
        return executionStatus == TaskStatus.Inactive;
    }

    public override void OnChildExecuted(TaskStatus childStatus)
    {
        executionStatus = childStatus;
        lastChildIndex = currentChildIndex;
    }

    public override void OnEnd()
    {
        executionStatus = TaskStatus.Inactive;
    }
}
