using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_TMP_Effect
{
    TEST_TMP_Chara chara = null;

    RoundTimer roundTimer = null;

    public void Apply(TEST_TMP_Chara appliedChara)
    {
        chara = appliedChara;
        roundTimer = RoundManager.Instance.CreateRoundTimer(5f, UpdateEffect, EndEffect);
    }

    private void UpdateEffect()
    {
        Debug.Log("Update effect on " + chara);
    }

    private void EndEffect()
    {
        Debug.Log("End effect on " + chara);
    }

    public void Progress()
    {
        roundTimer.ProgressRound(1f);
    }
}
