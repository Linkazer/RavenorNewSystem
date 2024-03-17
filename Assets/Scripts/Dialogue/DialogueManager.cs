using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private DialogueHandler dialogueHandler;

    private DialogueScriptable currentDialogue;
    private Action dialogueCallback;

    private int currentDialogueProgress = -1;

    public DialogueHandler CurrentHandler => dialogueHandler;

    public void SetCurrentHandler(DialogueHandler handler)
    {
        dialogueHandler = handler;
    }

    public void UnsetCurrentHandler(DialogueHandler handler)
    {
        if(dialogueHandler == handler)
        {
            dialogueHandler = null;
        }
    }

    public void PlayDialogue(DialogueScriptable dialogueToPlay, Action endDialogueCallback)
    {
        currentDialogueProgress = -1;

        dialogueCallback = endDialogueCallback;
        currentDialogue = dialogueToPlay;

        CurrentHandler.ShowDialogueHandler();

        PlayNextDialogueAction();
    }

    private void PlayNextDialogueAction()
    {
        currentDialogueProgress++;

        if(currentDialogueProgress >= currentDialogue.Actions.Length)
        {
            EndDialogue();
        }
        else
        {
            currentDialogue.Actions[currentDialogueProgress].DoAction(this, PlayNextDialogueAction);
        }
    }

    private void EndDialogue()
    {
        CurrentHandler.HideDialogueHandler();

        dialogueCallback?.Invoke();
        dialogueCallback = null;
        currentDialogue = null;
    }
}
