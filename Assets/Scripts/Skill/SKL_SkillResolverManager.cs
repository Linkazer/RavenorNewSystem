using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_SkillResolverManager : Singleton<SKL_SkillResolverManager>
{
    [SerializeField] private SKL_SkillActionBehavior[] skillActionBehaviors;

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
        PlayerActionManager.instance.AddLock(this);

        SKL_SkillResolver skillResolver = new SKL_SkillResolver(resolvingSkill, () => EndResolveSkill(callback));
        skillResolver.StartSkillAction(resolvingSkill.SkillData.GetFirstUsableSkillAction(resolvingSkill));
    }

    private void EndResolveSkill(Action callback)
    {
        PlayerActionManager.instance.RemoveLock(this);

        callback?.Invoke();
    }
}
