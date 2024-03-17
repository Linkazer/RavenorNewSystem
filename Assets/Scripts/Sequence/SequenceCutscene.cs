using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCutscene : Sequence
{
    protected override void OnStartAction()
    {
        PlayerEntityActionManager.Instance.AddLock(this);

        base.OnStartAction();
    }

    protected override void OnEndAction()
    {
        PlayerEntityActionManager.Instance.RemoveLock(this);

        base.OnEndAction();
    }
}
