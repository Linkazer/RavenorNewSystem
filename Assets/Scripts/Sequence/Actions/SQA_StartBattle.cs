using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_StartBattle : SequenceAction
{
    protected override void OnStartAction()
    {
        BattleRoundManager.Instance.StartBattle();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}
