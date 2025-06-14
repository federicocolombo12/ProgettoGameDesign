using System;
using System.Linq;
using UnityEngine;

public class UpgradeAbility : MonoBehaviour
{
    [SerializeField] PlayerTransformation transformationToUpgrade;
    [SerializeField] PlayerTransformation.AbilityType abilityToUpgrade;
    [SerializeField] Tutorial tutorialToShow;
    [SerializeField] TutorialEvents tutorialEvents;
    
    public static event Action<PlayerTransformation.AbilityType> OnAbilityUpgraded;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transformationToUpgrade.isUnlocked = true;
            transformationToUpgrade.AddAbility(abilityToUpgrade);
            tutorialEvents.RaiseEvent(tutorialToShow);
            OnAbilityUpgraded?.Invoke(abilityToUpgrade);
            Player.Instance.playerMovement.UpdateVariables();
            Destroy(gameObject, 0.5f);
            
        }
    }
}
