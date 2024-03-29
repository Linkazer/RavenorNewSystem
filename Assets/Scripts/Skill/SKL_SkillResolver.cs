using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_SkillResolver
{
    private SKL_ResolvingSkillData skillToResolve;

    private Action endSkillCallback;

    private List<SKL_SkillActionAnimation> runningAnimations = new List<SKL_SkillActionAnimation>();

    private Action postAnimationCallback; //TODO : Voir si on peut pas faire sans
    private Action removeLastSubResolverCallback; //TODO : Voir si on peut pas faire sans

    private List<SKL_SkillResolver> subResolvers = new List<SKL_SkillResolver>();

    public SKL_ResolvingSkillData SkillToResolve => skillToResolve;

    public SKL_SkillResolver(SKL_ResolvingSkillData skillData, Action callback)
    {
        endSkillCallback = callback;

        skillToResolve = skillData;
    }

    public void AddSubResolverFromData(SKL_ResolvingSkillData subResolverData, SKL_SkillAction subResolverAction)
    {
        SKL_SkillResolver subToAdd = null;
        subToAdd = new SKL_SkillResolver(subResolverData, () => RemoveSubResolver(subToAdd));
        subToAdd.StartSkillAction(subResolverAction);
    }

    public void AddSubResolver(SKL_SkillResolver subResolver)
    {
        subResolvers.Add(subResolver);
        //subResolver.endSkillCallback += () => RemoveSubResolver(subResolver);
    }

    public void RemoveSubResolver(SKL_SkillResolver subResolver)
    {
        if (subResolvers.Count > 0)
        {
            subResolvers.Remove(subResolver);

            if (subResolvers.Count == 0)
            {
                removeLastSubResolverCallback?.Invoke();
            }
        }
    }

    public void StartSkillAction(SKL_SkillAction toStart)
    {
        if (toStart.StartAnimations.Length > 0)
        {
            foreach (SKL_SkillActionAnimation anim in toStart.StartAnimations)
            {
                anim.PlayAnimation(toStart, this);
            }

            if (runningAnimations.Count > 0)
            {
                postAnimationCallback = () => TriggerSkillAction(toStart);
            }
            else
            {
                TriggerSkillAction(toStart);
            }
        }
        else
        {
            TriggerSkillAction(toStart);
        }
    }

    public void TriggerSkillAction(SKL_SkillAction toTrigger)
    {
        SKL_SkillActionBehavior spellBehavior = SKL_SkillResolverManager.Instance.GetBehaviorForType(toTrigger);

        spellBehavior.ResolveAction(toTrigger, this);
    }

    public void EndSkillAction(SKL_SkillAction nextAction)
    {
        if (nextAction != null)
        {
            StartSkillAction(nextAction);
        }
        else
        {
            if (runningAnimations.Count > 0)
            {
                postAnimationCallback = TryEndResolveSkill;
            }
            else if(subResolvers.Count > 0)
            {
                removeLastSubResolverCallback = TryEndResolveSkill;
            }
            else
            {
                TryEndResolveSkill();
            }
        }
    }

    private void TryEndResolveSkill()
    {
        if (runningAnimations.Count == 0 && subResolvers.Count == 0)
        {
            EndResolveSkill();
        }
    }

    public void EndResolveSkill()
    {
        endSkillCallback?.Invoke();
    }

    public void OnAnimationLaunch(SKL_SkillActionAnimation animationData)
    {
        if (!runningAnimations.Contains(animationData))
        {
            runningAnimations.Add(animationData);
        }
    }

    public void OnAnimationEnd(SKL_SkillActionAnimation endedAnimation)
    {
        if (runningAnimations.Contains(endedAnimation))
        {
            runningAnimations.Remove(endedAnimation);

            if (runningAnimations.Count == 0)
            {
                Action lastAction = postAnimationCallback;

                postAnimationCallback?.Invoke();

                if (lastAction == postAnimationCallback)
                {
                    postAnimationCallback = null;
                }
            }
        }
    }
}
