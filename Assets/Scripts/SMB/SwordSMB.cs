using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSMB : SceneLinkedSMB<PlayerBattle>
{
    public int actNumber;
    public string actName;
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actNumber == 5 && SLMonoBehaviour.isReset) return;
        SLMonoBehaviour.AddAct(actNumber, actName);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actNumber == 5 && SLMonoBehaviour.isReset)
        {
            SLMonoBehaviour.Reset();
            SLMonoBehaviour.isReset = false;
        }
    }
}
