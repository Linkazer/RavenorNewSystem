using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AS_LaunchProjectile : SKL_SkillAction
{
    [SerializeField] private SkillProjectile projectile;

    [SerializeField] private SKL_SkillActionChooser projectileReachTargetAction;

    public SkillProjectile Projectile => projectile;

    public SKL_SkillAction GetProjectileReachTargetAction(SKL_ResolvingSkillData resolvingSkillData)
    {
        return projectileReachTargetAction.GetFirstUsableAction(resolvingSkillData);
    }
}
