using UnityEngine;

public class DodgeSMB : SceneLinkedSMB<PlayerBattle>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SLMonoBehaviour.Dodge();
    }
}
