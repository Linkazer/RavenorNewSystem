using System.Collections.Generic;

public class SKL_AB_RollDice : SKL_SkillActionBehavior<SKL_AS_RollDice>
{
    protected override void ResolveAction(SKL_AS_RollDice actionToResolve, SKL_SkillResolver resolver)
    {
        bool didHit = false;

        Node casterNode = resolver.SkillToResolve.Caster?.CurrentNode;
        Node targetNode = resolver.SkillToResolve.TargetNode;

        SKL_SkillAction subAction = null;

        foreach (Node node in actionToResolve.Shape.GetZone(casterNode, targetNode))
        {
            SKL_ResolvingSkillData subResolverData = new SKL_ResolvingSkillData(resolver.SkillToResolve.SkillData, resolver.SkillToResolve.Caster, node);
            subAction = null;

            if (ResolveOnNode(actionToResolve, resolver.SkillToResolve, node, out subResolverData.dicesResult))
            {
                subAction = actionToResolve.GetTouchActionOnTarget(subResolverData);
               
                didHit = true;
            }
            else
            {
                subAction = actionToResolve.GetNoTouchActionOnTarget(subResolverData);
            }

            if (subAction != null)
            {
                resolver.AddSubResolverFromData(subResolverData, subAction);
            }
        }

        foreach(SKL_SkillActionAnimation anim in actionToResolve.OnDiceRollAnimation)
        {
            anim.PlayAnimation(actionToResolve, resolver);
        }

        SKL_SkillAction nextAction = null;

        if (didHit)
        {
            nextAction = actionToResolve.GetTouchAction(resolver.SkillToResolve);
        }
        else
        {
            nextAction = actionToResolve.GetNoTouchAction(resolver.SkillToResolve);
        }

        if (nextAction == null)
        {
            nextAction = actionToResolve.GetNextAction(resolver.SkillToResolve);
        }

        EndResolve(nextAction, resolver);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionToResolve"></param>
    /// <param name="resolvingData"></param>
    /// <param name="targetNode"></param>
    /// <returns>Did Hit someone</returns>
    private bool ResolveOnNode(SKL_AS_RollDice actionToResolve, SKL_ResolvingSkillData resolvingData, Node targetNode, out List<Dice> rolledDices)
    {
        rolledDices = new List<Dice>();

        List<EC_HealthHandler> hitableObjects = targetNode.GetEntityComponentsOnNode<EC_HealthHandler>();

        bool didHitAtLeastOne = false;

        foreach (EC_HealthHandler hitedObject in hitableObjects)
        {
            //TODO Spell Rework : Check target possible (Voir si on met pas �a directement dans la Shape)

            rolledDices = DiceManager.RollDices(resolvingData.Caster, actionToResolve.NumberDicesRoll, resolvingData.Caster != null ? resolvingData.Caster.Data.Accuracy : 0);

            RollDices(rolledDices, resolvingData.Caster, hitedObject, out bool didHit);

            //hitedObject.DisplayDiceResults(actionDices); //TODO : Feedback des r�sultats de d�s

            if (didHit)
            {
                didHitAtLeastOne = true;
            }
        }

        return didHitAtLeastOne;
    }

    private void RollDices(List<Dice> dicesToRoll, EC_SkillHandler caster, EC_HealthHandler target, out bool didHit)
    {
        didHit = false;

        float totalHits = 0;
        int currentOffensiveRerolls = -target.Data.DefensiveDisavantage;//Equivalent d'augmenter le max d'Offensive reroll disponible)
        int currentDefensiveRerolls = 0;
        if (caster != null)
        {
            currentDefensiveRerolls = -caster.Data.OffensiveDisavantage;
        }

        for (int i = 0; i < dicesToRoll.Count; i++)
        {
            totalHits += CheckDiceHit(caster, dicesToRoll[i], target.Data.Dodge, currentOffensiveRerolls < caster?.Data.OffensiveAdvantage, currentDefensiveRerolls < target.Data.DefensiveAdvantage, out bool usedOffensiveReroll, out bool usedDefensiveReroll);

            if (usedDefensiveReroll)
            {
                currentDefensiveRerolls++;
                dicesToRoll[i].rerolledForDefensive = true;
            }

            if (usedOffensiveReroll)
            {
                currentOffensiveRerolls++;
                dicesToRoll[i].rerolledForOffensive = true;
            }
        }

        if (totalHits > 0 || dicesToRoll.Count == 0)
        {
            didHit = true;
        }
    }

    private int CheckDiceHit(EC_SkillHandler caster, Dice dice, int defense, bool hasOffensiveReroll, bool hasDefensiveReroll, out bool usedOffensiveReroll, out bool usedDefensiveReroll)
    {
        int toReturn = 0;

        usedDefensiveReroll = false;
        usedOffensiveReroll = false;

        if (dice.Result > defense)
        {
            if (hasDefensiveReroll)
            {
                usedDefensiveReroll = true;
                dice.Roll(caster);
                return CheckDiceHit(caster, dice, defense, hasOffensiveReroll, false, out usedOffensiveReroll, out bool def);
            }
            else
            {
                dice.DoesHit = true;
                toReturn = 1;
            }
        }
        else if (hasOffensiveReroll)
        {
            usedOffensiveReroll = true;
            dice.Roll(caster);
            return CheckDiceHit(caster, dice, defense, false, hasDefensiveReroll, out bool off, out usedDefensiveReroll);
        }

        return toReturn;
    }
}
