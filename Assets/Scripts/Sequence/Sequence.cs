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

    /*[ContextMenu("Fill Sequence")]
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
*/
    
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
