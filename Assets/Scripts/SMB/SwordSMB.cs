using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSMB : SceneLinkedSMB<PlayerBattle>
{
    public int actNumber;
    public string actName;
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actNumber == 5 && SLMonoBehaviour.IsTurnReset)
        {
            SLMonoBehaviour.IsTurnReset = false;
            return;
        }
        SLMonoBehaviour.AddAct(actNumber, actName);
    }
}
