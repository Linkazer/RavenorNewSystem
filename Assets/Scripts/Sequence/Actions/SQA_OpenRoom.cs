using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_OpenRoom : SequenceAction
{
    [SerializeField] private RoomHandler roomToOpen;

    protected override void OnStartAction()
    {
        roomToOpen.OpenRoom(EndAction);
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}
