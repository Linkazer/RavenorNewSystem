using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_ChallengeRoll : SequenceAction
{
    [Serializable]
    private struct ChallengeSuccessPossibility
    {
        public int minimumSuccessNeeded;
        [SerializeReference, ReferenceEditor(typeof(SequenceAction))] public SequenceAction actionOnSuccess;
    }

    [SerializeField] private ChallengeRollData challenge;
    [SerializeField] private ChallengeSuccessPossibility[] possiblesResults;

    private SequenceContext currentContext;

    protected override void OnStartAction(SequenceContext context)
    {
        Entity challengedEntity = null;

        if(context.interactionEntity != null)
        {
            challengedEntity = context.interactionEntity;
        }

        if(challengedEntity != null && challengedEntity.TryGetEntityComponentOfType(out EC_TraitHandler traitsHandler))
        {
            InitializeChallenge(traitsHandler);
        }
        else
        {
            EndAction(context);
        }
    }

    protected override void OnEndAction(SequenceContext context)
    {
        
    }

    protected override void OnSkipAction(SequenceContext context)
    {
        
    }

    private void InitializeChallenge(EC_TraitHandler traitsHandler)
    {
        ChallengeManager.Instance.InitializeChallenge(challenge, traitsHandler, OnChallengeResolved);
    }

    private void OnChallengeResolved(int result)
    {
        Debug.Log(result);

        SequenceAction resultAction = null;

        foreach (ChallengeSuccessPossibility successPossibility in possiblesResults)
        {
            if (result >= successPossibility.minimumSuccessNeeded)
            {
                resultAction = successPossibility.actionOnSuccess;
                break;
            }
        }

        if (resultAction != null)
        {
            resultAction.StartAction(currentContext, () => EndAction(currentContext));
        }
        else
        {
            EndAction(currentContext);
        }
    }
}
