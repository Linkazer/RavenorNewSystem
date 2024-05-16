using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : SequenceAction
{
    [Serializable]
    public class SequenceStep
    {
        public SequenceAction mainAction;
        public List<SequenceAction> secondaryActions = new List<SequenceAction>();
    }

    [SerializeField] private List<SequenceStep> steps = new List<SequenceStep>();

    private int currentStep = 0;

    [ContextMenu("Fill Sequence")]
    public void FillSequence()
    {
        steps = new List<SequenceStep>();

        foreach (Transform child in transform)
        {
            SequenceStep newStep = new SequenceStep();

            newStep.mainAction = child.GetComponent<SequenceAction>();

            if(newStep.mainAction != null && newStep.mainAction.enabled && newStep.mainAction.gameObject.activeSelf)
            {
                if (newStep.mainAction as Sequence == null && newStep.mainAction as SequenceCutscene == null)
                {
                    foreach (Transform secondaryChild in child)
                    {
                        if (secondaryChild.gameObject.activeSelf)
                        {
                            SequenceAction toAdd = secondaryChild.GetComponent<SequenceAction>();

                            if (toAdd != null && toAdd.enabled)
                            {
                                newStep.secondaryActions.Add(toAdd);
                            }
                        }
                    }
                }
                else
                {
                    (newStep.mainAction as Sequence).FillSequence();
                }

                steps.Add(newStep);
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
    }

    public void PlaySequence(Action callback)
    {
        StartAction(new SequenceContext(this), callback);
    }

    public void PlaySequence(Action callback, Entity entity)
    {
        StartAction(new SequenceContext(this, entity), callback);
    }

    private void NextStep(SequenceContext context)
    {
        currentStep++;

        if (currentStep >= steps.Count)
        {
            EndAction(context);
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

    protected override void OnStartAction(SequenceContext context)
    {
        currentStep = -1;

        NextStep(context);
    }

    protected override void OnEndAction(SequenceContext context)
    {
        
    }

    protected override void OnSkipAction(SequenceContext context)
    {
        throw new NotImplementedException();
    }
}
