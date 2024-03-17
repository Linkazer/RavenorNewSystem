using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_MoveCharacter : SequenceAction
{
    [SerializeField] private EC_Movement toMove;
    [SerializeField] private Transform movementStart;
    [SerializeField] private Transform movementTarget;

    protected override void OnStartAction()
    {
        toMove.TryMoveToDestination(movementTarget.position, EndAction);
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
       
    }
}
