using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_PlayDialogue : SequenceAction
{
    [SerializeField] private DialogueScriptable dialogueToPlay;

    protected override void OnStartAction(SequenceContext context)
    {
        DialogueManager.Instance.PlayDialogue(dialogueToPlay, () => EndAction(context));
    }

    protected override void OnEndAction(SequenceContext context)
    {
        
    }

    protected override void OnSkipAction(SequenceContext context)
    {
        
    }
}
