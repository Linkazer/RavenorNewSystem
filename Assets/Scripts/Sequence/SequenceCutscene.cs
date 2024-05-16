using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCutscene : Sequence
{
    protected override void StartSequence(SequenceContext context, Action callback)
    {
        PlayerActionManager.Instance.AddLock(this);

        base.StartSequence(context, callback);
    }

    protected override void EndSequence()
    {
        PlayerActionManager.Instance.RemoveLock(this);

        base.EndSequence();
    }
}
