using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_PlaySequence : SequenceAction
{
    [SerializeField] private Sequence sequenceToPlay;

    protected override void OnStartAction(SequenceContext context)
    {
        sequenceToPlay.PlaySequence(() => EndAction(context));
    }

    protected override void OnEndAction(SequenceContext context)
    {
        
    }

    protected override void OnSkipAction(SequenceContext context)
    {
       
    }
}
