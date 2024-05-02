using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AB_RemoveRessource : SKL_SkillActionBehavior<SKL_AS_RemoveRessource>
{
    protected override void ResolveAction(SKL_AS_RemoveRessource actionToResolve, SKL_SkillResolver resolver)
    {
        if(resolver.SkillToResolve.Caster != null)
        {
            if (resolver.SkillToResolve.Caster.RessourceUsed != null && resolver.SkillToResolve.Caster.RessourceUsed.RessourceType == actionToResolve.RessourceType)
            {
                resolver.SkillToResolve.Caster.RessourceUsed.RemoveRessource(actionToResolve.AmountToRemove);
            }
        }

        EndResolve(actionToResolve.GetNextAction(resolver.SkillToResolve), resolver);
    }
}
