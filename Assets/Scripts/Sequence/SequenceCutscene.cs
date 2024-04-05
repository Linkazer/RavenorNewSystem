using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCutscene : Sequence
{
    protected override void OnStartAction()
    {
        PlayerActionManager.Instance.AddLock(this);

        base.OnStartAction();
    }

    protected override void OnEndAction()
    {
        PlayerActionManager.Instance.RemoveLock(this);

        base.OnEndAction();
    }
}
