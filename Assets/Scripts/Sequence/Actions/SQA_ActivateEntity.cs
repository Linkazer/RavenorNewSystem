using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SQA_ActivateEntity : SequenceAction
{
    [SerializeField] private Entity[] entityToHandle;
    [SerializeField] private bool doesActivate;
    [SerializeField] private bool TEMP_isControllable = true;

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

            if (entity is CharacterEntity && TEMP_isControllable)
            {
                ControllableTeamHandler.Instance.AddCharacter(entity as CharacterEntity);
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
