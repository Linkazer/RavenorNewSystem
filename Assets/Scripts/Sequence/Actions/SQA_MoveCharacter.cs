using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_MoveCharacter : SequenceAction
{
    [SerializeField] private EC_Movement toMove;
    [SerializeField] private Transform movementStart;
    [SerializeField] private Transform movementTarget;

    protected override void OnStartAction(SequenceContext context)
    {
        toMove.TryMoveToDestination(movementTarget.position, () => EndAction(context));
    }

    protected override void OnEndAction(SequenceContext context)
    {
        
    }

    protected override void OnSkipAction(SequenceContext context)
    {
       
    }
}
