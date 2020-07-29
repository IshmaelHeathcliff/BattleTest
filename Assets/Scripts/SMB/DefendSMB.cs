using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendSMB : SceneLinkedSMB<PlayerBattle>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SLMonoBehaviour.Defend();
    }
}
