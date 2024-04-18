using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_ActivateEntity : SequenceAction
{
    [SerializeField] private Entity[] entityToHandle;
    [SerializeField] private bool doesActivate;

    protected override void OnStartAction()
    {
        foreach(Entity entity in entityToHandle)
        {
            if(doesActivate)
            {
                entity.Activate();
            }
            else
            {
                entity.Deactivate();
            }
        }

        EndAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}
