using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReferencePicker;

public class SQA_DoCheck : SequenceAction
{
    [Serializable]
    private class CheckAction
    {
        [SerializeReference, ReferenceEditor(typeof(Checker))] public Checker[] checkers;
        public SequenceAction actionOnValidCheck;

        public bool IsCheckValid()
        {
            foreach(Checker checker in checkers)
            {
                if(!checker.IsCheckValid())
                {
                    return false;
                }
            }

            return true;
        }
    }

    [SerializeField] private CheckAction[] checks;
    [SerializeField] private SequenceAction actionOnNoValidCheck;

    protected override void OnStartAction()
    {
        foreach(CheckAction check in checks)
        {
            if(check.IsCheckValid())
            {
                if (check.actionOnValidCheck != null)
                {
                    check.actionOnValidCheck.StartAction(EndAction);
                }
                else
                {
                    EndAction();
                }
                return;
            }
        }

        if (actionOnNoValidCheck != null)
        {
            actionOnNoValidCheck.StartAction(EndAction);
        }
        else
        {
            EndAction();
        }
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}