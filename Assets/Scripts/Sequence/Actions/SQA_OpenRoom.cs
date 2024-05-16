using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_OpenRoom : SequenceAction
{
    [SerializeField] private RoomHandler roomToOpen;

    protected override void OnStartAction(SequenceContext context)
    {
        roomToOpen.OpenRoom(() => EndAction(context));
    }

    protected override void OnEndAction(SequenceContext context)
    {
        
    }

    protected override void OnSkipAction(SequenceContext context)
    {
        
    }
}
