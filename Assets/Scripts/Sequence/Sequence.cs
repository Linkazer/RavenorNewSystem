using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReferencePicker;

public class Sequence : MonoBehaviour
{
    [Serializable]
    public class SequenceStep
    {
        [SerializeReference, ReferenceEditor(typeof(SequenceAction))] public SequenceAction mainAction;
        [SerializeReference, ReferenceEditor(typeof(SequenceAction))] public SequenceAction[] secondaryActions;
    }

    [SerializeField] private List<SequenceStep> steps = new List<SequenceStep>();

    private int currentStep = 0;

    private Action endCallback;

    public void PlaySequence(Action callback)
    {
        StartSequence(new SequenceContext(this), callback);
    }

    public void PlaySequence(Action callback, Entity entity)
    {
        StartSequence(new SequenceContext(this, entity), callback);
    }

    private void NextStep(SequenceContext context)
    {
        currentStep++;

        if (currentStep >= steps.Count)
        {
            EndSequence();
        }
        else
        {
            foreach (SequenceAction act in steps[currentStep].secondaryActions)
            {
                act.StartAction(context);
            }

            steps[currentStep].mainAction.StartAction(context, () => NextStep(context));
        }
    }

    protected virtual void StartSequence(SequenceContext context, Action callback)
    {
        endCallback = callback;

        currentStep = -1;

        NextStep(context);
    }

    protected virtual void EndSequence()
    {
        endCallback?.Invoke();
    }
}
