using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handle Dialogue logic.
/// </summary>
public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private DialogueHandler dialogueHandler;

    private DialogueScriptable currentDialogue;
    private Action dialogueCallback;

    private int currentDialogueProgress = -1;

    public DialogueHandler CurrentHandler => dialogueHandler;

    /// <summary>
    /// Set the current Dialogue UI.
    /// </summary>
    /// <param name="handler">The Dialogue UI to set.</param>
    public void SetCurrentHandler(DialogueHandler handler)
    {
        dialogueHandler = handler;
    }

    /// <summary>
    /// Unset the current Dialogue UI.
    /// </summary>
    /// <param name="handler">The Dialogue UI to unset.</param>
    public void UnsetCurrentHandler(DialogueHandler handler)
    {
        if(dialogueHandler == handler)
        {
            dialogueHandler = null;
        }
    }

    /// <summary>
    /// Play a Dialogue.
    /// </summary>
    /// <param name="dialogueToPlay">The Dialogue to play.</param>
    /// <param name="endDialogueCallback">The method to call at the end of the Dialogue.</param>
    public void PlayDialogue(DialogueScriptable dialogueToPlay, Action endDialogueCallback)
    {
        currentDialogueProgress = -1;

        dialogueCallback = endDialogueCallback;
        currentDialogue = dialogueToPlay;

        CurrentHandler.ShowDialogueHandler();

        PlayNextDialogueAction();
    }

    /// <summary>
    /// Play the next Action of the Dialogue.
    /// </summary>
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

    /// <summary>
    /// End the current Dialogue.
    /// </summary>
    private void EndDialogue()
    {
        CurrentHandler.HideDialogueHandler();

        dialogueCallback?.Invoke();
        dialogueCallback = null;
        currentDialogue = null;
    }
}
