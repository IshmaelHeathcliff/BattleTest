using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSMB : SceneLinkedSMB<Enemy>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SLMonoBehaviour.Attack();
        SLMonoBehaviour.Indicate();
        SLMonoBehaviour.EndTurn();
    }
}
