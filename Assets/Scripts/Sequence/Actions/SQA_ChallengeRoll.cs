using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_ChallengeRoll : SequenceAction
{
    private const int BaseDiceAmountForChallenge = 3;

    [Serializable]
    private struct ChallengeSuccessPossibility
    {
        public int minimumSuccessNeeded;
        public SequenceAction actionOnSuccess;
    }

    [Serializable]
    private class ChallengeRoll
    {
        public EntityTraits traitWanted;
        public int difficulty;
        public ChallengeSuccessPossibility[] successPossibilities;
    }

    [SerializeField] private ChallengeRoll challenge;

    protected override void OnStartAction(SequenceContext context)
    {
        Entity challengedEntity = null;

        if(context.interactionEntity != null)
        {
            challengedEntity = context.interactionEntity;
        }

        if(challengedEntity != null && challengedEntity.TryGetEntityComponentOfType(out EC_TraitHandler traitsHandler))
        {
            SequenceAction challengeResult = GetChallengeResultAction(challenge, traitsHandler);

            if(challengeResult != null)
            {
                challengeResult.StartAction(context, () => EndAction(context));
            }
            else
            {
                EndAction(context);
            }
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

    private SequenceAction GetChallengeResultAction(ChallengeRoll challengeToRoll, EC_TraitHandler entityChallenged)
    {
        int diceResultBonus = 0;

        if(entityChallenged != null )
        {
            diceResultBonus = entityChallenged.GetTraitValue(challengeToRoll.traitWanted);

        }

        List<Dice> challengeDices = DiceManager.RollDices(this, BaseDiceAmountForChallenge, diceResultBonus);

        int result = 0;

        foreach(Dice dice in challengeDices)
        {
            if(dice.Result > challengeToRoll.difficulty)
            {
                result++;
            }
        }

        foreach(ChallengeSuccessPossibility successPossibility in challengeToRoll.successPossibilities)
        {
            if(result >= successPossibility.minimumSuccessNeeded)
            {
                return successPossibility.actionOnSuccess;
            }
        }

        return null;
    }
}
