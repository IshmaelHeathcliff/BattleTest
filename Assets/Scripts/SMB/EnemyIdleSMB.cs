using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleSMB : SceneLinkedSMB<Enemy>
{
    static readonly int _Attack = Animator.StringToHash("Attack");

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!PlayerBattle.Instance.IsPlayerTurn)
        {
            animator.SetTrigger(_Attack);
        }
    }
}
