using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_Animation_Fx : SKL_SkillActionAnimation
{
    [SerializeField] private Poolable_FX fxToPlay = null;

    public override float AnimationDuration => fxToPlay != null ? fxToPlay.CallbackDelay : animationDuration;

    protected override void OnPlayAnimation(SKL_SkillAction skillAction, SKL_SkillResolver resolver)
    {
        Action animationCallback = () => EndAnimation(resolver);

        foreach (Node node in GetAnimationNodes(skillAction, resolver))
        {
            PoolManager.InstatiatePoolableAtPosition(fxToPlay, node.WorldPosition, animationCallback);
            animationCallback = null;
        }

        if (animationCallback != null)
        {
            animationCallback.Invoke();
        }
    }
}
