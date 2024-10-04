using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_Animation_EntityAnimation : SKL_SkillActionAnimation
{
    [SerializeField] private string animationName;

    private Timer animationTimer = null;

    public override float AnimationDuration => animationDuration;

    protected override void OnPlayAnimation(SKL_SkillAction skillAction, SKL_SkillResolver resolver)
    {
        foreach (Node node in GetAnimationNodes(skillAction, resolver))
        {
            foreach(EC_Renderer renderer in node.GetEntityComponentsOnNode<EC_Renderer>())
            {
                if(renderer.AnimHandler != null)
                {
                    renderer.AnimHandler.PlayAnimation(animationName, new Action[] { () => EndAnimation(resolver) }, resolver.SkillToResolve);
                }
            }
        }

        //animationTimer = TimerManager.CreateGameTimer(animationDuration, () => EndAnimation(resolver)); RECUP ANIM EVENT
    }

    protected override void EndAnimation(SKL_SkillResolver resolver)
    {
        base.EndAnimation(resolver);

        animationTimer?.Stop();
        animationTimer = null;
    }
}
