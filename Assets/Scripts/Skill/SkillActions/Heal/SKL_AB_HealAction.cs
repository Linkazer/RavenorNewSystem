using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AB_HealAction : SKL_SkillActionBehavior<SKL_AS_HealAction>
{
    protected override void ResolveAction(SKL_AS_HealAction actionToResolve, SKL_SkillResolver resolver)
    {
        Node casterNode = resolver.SkillToResolve.Caster?.CurrentNode;
        Node targetNode = resolver.SkillToResolve.TargetNode;

        foreach (Node node in actionToResolve.Shape.GetZone(casterNode, targetNode))
        {
            ApplyHealOnNode(node, actionToResolve, resolver.SkillToResolve);
        }

        foreach (SKL_SkillActionAnimation anim in actionToResolve.HealAnimation)
        {
            anim.PlayAnimation(actionToResolve, resolver);
        }

        EndResolve(actionToResolve.GetNextAction(resolver.SkillToResolve), resolver);
    }

    private void ApplyHealOnNode(Node nodeToApplyDamageOn, SKL_AS_HealAction actionToResolve, SKL_ResolvingSkillData resolvingSkillData)
    {
        List<EC_HealthHandler> healthHandlersOnNode = nodeToApplyDamageOn.GetEntityComponentsOnNode<EC_HealthHandler>();

        foreach (EC_HealthHandler hitedHealthHandlers in healthHandlersOnNode)
        {
            foreach (SKL_DamageData damageData in actionToResolve.HealsData)
            {
                ApplyHealOnTarget(damageData, hitedHealthHandlers, resolvingSkillData);
            }
        }
    }

    private void ApplyHealOnTarget(SKL_DamageData damageToApply, EC_HealthHandler targetHealth, SKL_ResolvingSkillData resolvingSkillData)
    {
        int healAmount = damageToApply.Origin.GetDamageAmount(resolvingSkillData);

        switch (damageToApply.DamageType)
        {
            case SKL_DamageType.Heal:
                targetHealth.GainHealth(healAmount);
                break;
            case SKL_DamageType.RegenArmor:
                targetHealth.GainArmor(healAmount);
                break;
        }
    }
}
