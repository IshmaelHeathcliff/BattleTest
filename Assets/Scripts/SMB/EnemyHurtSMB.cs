using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtSMB : SceneLinkedSMB<Enemy>
{
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SLMonoBehaviour.Health <= 0)
        {
            SceneManagement.Instance.Win();
            return;
        }
        
        SLMonoBehaviour.StartTurn();
    }
}
