using UnityEngine;

public class OnEnterPlay : StateMachineBehaviour
{
    [SerializeField] SfxData sfxData;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.Instance.sfxChannel.RaiseEvent(sfxData);
    }

    
}
