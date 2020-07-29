using UnityEngine;

public class StrikeSMB : SceneLinkedSMB<PlayerBattle>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SLMonoBehaviour.Strike();
    }
}
