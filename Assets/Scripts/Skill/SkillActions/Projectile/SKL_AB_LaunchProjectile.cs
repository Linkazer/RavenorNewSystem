using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AB_LaunchProjectile : SKL_SkillActionBehavior<SKL_AS_LaunchProjectile>
{
    protected override void ResolveAction(SKL_AS_LaunchProjectile actionToResolve, SKL_SkillResolver resolver)
    {
        SkillProjectile projectile = Instantiate(actionToResolve.Projectile, transform);

        projectile.transform.position = resolver.SkillToResolve.Caster.transform.position;

        projectile.AskMoveToDestination(resolver.SkillToResolve.TargetNode.WorldPosition, () => OnProjectileReachDestination(actionToResolve, resolver));

    }

    private void OnProjectileReachDestination(SKL_AS_LaunchProjectile actionToResolve, SKL_SkillResolver resolver)
    {
        SKL_SkillAction nextAction = actionToResolve.GetProjectileReachTargetAction(resolver.SkillToResolve);

        EndResolve(nextAction, resolver);
    }
}
