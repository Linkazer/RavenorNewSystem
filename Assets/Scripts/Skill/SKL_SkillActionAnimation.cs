using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKL_AnimationTarget
{
    FullZone,
    TargetNode,
    CasterNode,
    HandlersInZone,
}

[Serializable]
public abstract class SKL_SkillActionAnimation
{
    [SerializeField] protected SKL_AnimationTarget animationTarget;
    [SerializeField] protected float animationDuration;

    public abstract float AnimationDuration { get; }

    public void PlayAnimation(SKL_SkillAction skillAction, SKL_SkillResolver resolver)
    {
        resolver.OnAnimationLaunch(this);

        OnPlayAnimation(skillAction, resolver);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skillAction">Le SpellAction qui contient l'animation.</param>
    /// <param name="resolver">Le Resolver de l'action.</param>
    protected abstract void OnPlayAnimation(SKL_SkillAction skillAction, SKL_SkillResolver resolver);

    protected virtual void EndAnimation(SKL_SkillResolver resolver)
    {
        resolver.OnAnimationEnd(this);
    }

    protected List<Node> GetAnimationNodes(SKL_SkillAction skillAction, SKL_SkillResolver resolver)
    {
        switch (animationTarget)
        {
            case SKL_AnimationTarget.FullZone:
                return skillAction.Shape.GetZone(resolver.SkillToResolve.Caster?.CurrentNode, resolver.SkillToResolve.TargetNode);
            case SKL_AnimationTarget.HandlersInZone:
                List<Node> zoneNode = skillAction.Shape.GetZone(resolver.SkillToResolve.Caster?.CurrentNode, resolver.SkillToResolve.TargetNode);

                for (int i = 0; i < zoneNode.Count; i++)
                {
                    if (zoneNode[i].EntitiesOnNode.Count == 0)
                    {
                        zoneNode.RemoveAt(i);
                        i--;
                    }
                }
                return zoneNode;
            case SKL_AnimationTarget.CasterNode:
                return new List<Node>() { resolver.SkillToResolve.Caster.CurrentNode };
            case SKL_AnimationTarget.TargetNode:
                return new List<Node>() { resolver.SkillToResolve.TargetNode };
        }

        return new List<Node>();
    }
}
