using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialEvents", menuName = "Scriptable Objects/TutorialEvents")]
public class TutorialEvents : ScriptableObject
{
    public Action<Tutorial> OnTriggerTutorialRequested;

    public void RaiseEvent(Tutorial tutorialMaterial)
    {
        OnTriggerTutorialRequested?.Invoke(tutorialMaterial);
    }
}
