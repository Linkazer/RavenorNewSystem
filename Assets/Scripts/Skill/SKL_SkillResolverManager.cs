using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_SkillResolverManager : Singleton<SKL_SkillResolverManager>
{
    [SerializeField] private SKL_SkillActionBehavior[] skillActionBehaviors;

    private List<SKL_SkillResolver> currentlyRunningResolvers = new List<SKL_SkillResolver>(); //TODO : Check l'utilit�

    public SKL_SkillActionBehavior GetBehaviorForType(SKL_SkillAction actionSearchFor)
    {
        if(actionSearchFor != null)
        {
            for (int i = 0; i < skillActionBehaviors.Length; i++)
            {
                if (skillActionBehaviors[i].GetSkillType() == actionSearchFor.GetType())
                {
                    return skillActionBehaviors[i];
                }
            }
        }

        return null;
    }

    public void ResolveSpell(SKL_ResolvingSkillData resolvingSkill, Action callback)
    {
        SKL_SkillResolver skillResolver = new SKL_SkillResolver(resolvingSkill, callback);
        skillResolver.StartSkillAction(resolvingSkill.SkillData.GetFirstUsableSkillAction(resolvingSkill));
        //currentlyRunningResolvers.Add(new SKL_SkillResolver(resolvingSkill, callback));
    }

    private void EndResolve(SKL_SkillResolver toEnd)
    {
        //currentlyRunningResolvers.Remove(toEnd);
    }
}