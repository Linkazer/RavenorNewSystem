using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the behavior of a Dialogue Action.
/// </summary>
[Serializable]
public abstract class DialogueAction
{
    private Action endActionCallback;

    protected DialogueManager dialogueHandler;

    /// <summary>
    /// Do the Dialogue Action
    /// </summary>
    /// <param name="handler">The DialogueManager used.</param>
    /// <param name="callback">The method to call at the end of the Dialogue Action.</param>
    public void DoAction(DialogueManager handler, Action callback)
    {
        dialogueHandler = handler;
        endActionCallback = callback;

        OnDoAction();
    }

    /// <summary>
    /// Do specific things when doing the action depending on the Action type.
    /// </summary>
    protected abstract void OnDoAction();

    /// <summary>
    /// Called to end the Dialogue Action.
    /// </summary>
    public void EndAction()
    {
        endActionCallback?.Invoke();
        endActionCallback = null;

        OnEndAction();
    }

    /// <summary>
    /// Do specific things when the action is done depending on the Action type.
    /// </summary>
    protected abstract void OnEndAction();
}
