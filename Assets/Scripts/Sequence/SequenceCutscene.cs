using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCutscene : Sequence
{
    protected override void OnStartAction(SequenceContext context)
    {
        PlayerActionManager.Instance.AddLock(this);

        base.OnStartAction(context);
    }

    protected override void OnEndAction(SequenceContext context)
    {
        PlayerActionManager.Instance.RemoveLock(this);

        base.OnEndAction(context);
    }
}
