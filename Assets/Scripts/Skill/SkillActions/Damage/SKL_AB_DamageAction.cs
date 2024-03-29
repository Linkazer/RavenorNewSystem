using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AB_DamageAction : SKL_SkillActionBehavior<SKL_AS_DamageAction>
{
    private const int ArmorLostOnDamageTaken = 1;

    protected override void ResolveAction(SKL_AS_DamageAction actionToResolve, SKL_SkillResolver resolver)
    {
        Node casterNode = resolver.SkillToResolve.Caster?.CurrentNode;
        Node targetNode = resolver.SkillToResolve.TargetNode;

        foreach (Node node in actionToResolve.Shape.GetZone(casterNode, targetNode))
        {
            ApplyDamageOnNode(node, actionToResolve, resolver.SkillToResolve);
        }

        foreach (SKL_SkillActionAnimation anim in actionToResolve.DamageAnimations)
        {
            anim.PlayAnimation(actionToResolve, resolver);
        }

        EndResolve(actionToResolve.GetNextAction(resolver.SkillToResolve), resolver);
    }

    private void ApplyDamageOnNode(Node nodeToApplyDamageOn, SKL_AS_DamageAction actionToResolve, SKL_ResolvingSkillData resolvingSkillData)
    {
        List<EC_HealthHandler> healthHandlersOnNode = nodeToApplyDamageOn.GetEntityComponentsOnNode<EC_HealthHandler>();

        foreach(EC_HealthHandler hitedHealthHandlers in healthHandlersOnNode)
        {
            foreach(SKL_DamageData damageData in actionToResolve.DamagesData)
            {
                ApplyDamageOnTarget(damageData, hitedHealthHandlers, resolvingSkillData);
            }
        }
    }

    private void ApplyDamageOnTarget(SKL_DamageData damageToApply, EC_HealthHandler targetHealth, SKL_ResolvingSkillData resolvingSkillData)
    {
        int damageAmount = damageToApply.Origin.GetDamageAmount(resolvingSkillData);

        Debug.Log("Damage done : " + damageAmount);

        switch(damageToApply.DamageType)
        {
            case SKL_DamageType.Normal:
                damageAmount -= targetHealth.CurrentArmor;

                if(damageAmount < 0)
                {
                    damageAmount = 0;
                }

                targetHealth.LoseHealth(damageAmount);
                targetHealth.LoseArmor(ArmorLostOnDamageTaken);
                break;
            case SKL_DamageType.IgnoreArmor:
                targetHealth.LoseHealth(damageAmount);
                break;
            case SKL_DamageType.PierceArmor:
                targetHealth.LoseArmor(damageAmount);
                break;
        }
    }
}
