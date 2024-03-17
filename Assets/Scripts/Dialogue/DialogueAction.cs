using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DialogueAction
{
    private Action endActionCallback;

    protected DialogueManager dialogueHandler;

    public void DoAction(DialogueManager handler, Action callback)
    {
        dialogueHandler = handler;
        endActionCallback = callback;

        OnDoAction();
    }

    protected abstract void OnDoAction();

    public void EndAction()
    {
        endActionCallback?.Invoke();
        endActionCallback = null;

        OnEndAction();
    }

    protected abstract void OnEndAction();
}
