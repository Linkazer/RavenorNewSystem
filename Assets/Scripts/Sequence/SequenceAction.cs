using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceAction : MonoBehaviour
{
    private Action endCallback;

    public void StartAction(SequenceContext context)
    {
        endCallback = null;

        OnStartAction(context);
    }

    public void StartAction(SequenceContext context, Action callback)
    {
        endCallback = callback;

        OnStartAction(context);
    }

    public void EndAction(SequenceContext context)
    {
        OnEndAction(context);

        endCallback?.Invoke();
    }

    public void SkipAction(SequenceContext context, Action callback)
    {
        OnSkipAction(context);
    }

    //Start action
    protected abstract void OnStartAction(SequenceContext context);

    //End Action
    protected abstract void OnEndAction(SequenceContext context);

    //Skip action
    protected abstract void OnSkipAction(SequenceContext context);
}
