using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAction_DisplayText : DialogueAction
{
    [SerializeField, SerializeReference] private DialogueSpeaker speaker;
    [SerializeField] private RVN_Text text;

    protected override void OnDoAction()
    {
        dialogueHandler.CurrentHandler.DisplayDialogueText(speaker, text.GetText(), EndAction);
    }

    protected override void OnEndAction()
    {
        
    }
}
