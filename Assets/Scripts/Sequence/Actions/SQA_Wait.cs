using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_Wait : SequenceAction
{
    [SerializeField] private float waitTime;

    private Timer waitTimer;

    protected override void OnStartAction(SequenceContext context)
    {
        waitTimer = TimerManager.CreateGameTimer(waitTime, () => EndAction(context));
    }

    protected override void OnEndAction(SequenceContext context)
    {
        waitTimer?.Stop();
        waitTimer = null;
    }

    protected override void OnSkipAction(SequenceContext context)
    {
        waitTimer?.Execute();
        waitTimer = null;
    }
}
