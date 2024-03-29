using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_PlayDialogue : SequenceAction
{
    [SerializeField] private DialogueScriptable dialogueToPlay;

    protected override void OnStartAction()
    {
        DialogueManager.Instance.PlayDialogue(dialogueToPlay, EndAction);
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }
}
