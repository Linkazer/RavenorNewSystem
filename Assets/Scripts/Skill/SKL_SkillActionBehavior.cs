using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SKL_SkillActionBehavior : MonoBehaviour
{
    public delegate void EndActionCallback(SKL_SkillAction actionToPlayNext);

    public abstract Type GetSkillType();

    public abstract void ResolveAction(SKL_SkillAction actionToResolve, SKL_SkillResolver resolver);
}

public abstract class SKL_SkillActionBehavior<T> : SKL_SkillActionBehavior where T : SKL_SkillAction
{
    public override Type GetSkillType()
    {
        return typeof(T);
    }

    public override void ResolveAction(SKL_SkillAction actionToResolve, SKL_SkillResolver resolver)
    {
        ResolveAction(actionToResolve as T, resolver);
    }

    protected abstract void ResolveAction(T actionToResolve, SKL_SkillResolver resolver);

    protected void EndResolve(SKL_SkillAction nextAction, SKL_SkillResolver resolver)
    {
        resolver.TryEndSkillAction(nextAction);
    }
}
