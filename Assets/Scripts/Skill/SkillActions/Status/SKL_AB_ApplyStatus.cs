using System.Collections.Generic;

public class SKL_AB_ApplyStatus : SKL_SkillActionBehavior<SKL_AS_ApplyStatus>
{
    protected override void ResolveAction(SKL_AS_ApplyStatus actionToResolve, SKL_SkillResolver resolver)
    {
        Node casterNode = resolver.SkillToResolve.Caster?.CurrentNode;
        Node targetNode = resolver.SkillToResolve.TargetNode;

        foreach (Node node in actionToResolve.Shape.GetZone(casterNode, targetNode))
        {
            ApplyStatusOnNode(node, actionToResolve, resolver.SkillToResolve);
        }

        foreach (SKL_SkillActionAnimation anim in actionToResolve.ApplyStatusAnimations)
        {
            anim.PlayAnimation(actionToResolve, resolver);
        }

        EndResolve(actionToResolve.GetNextAction(resolver.SkillToResolve), resolver);
    }

    private void ApplyStatusOnNode(Node targetNode, SKL_AS_ApplyStatus actionToResolve, SKL_ResolvingSkillData resolvingSkill)
    {
        List<EC_StatusHandler> statusHandlersOnNode = targetNode.GetEntityComponentsOnNode<EC_StatusHandler>();

        foreach (EC_StatusHandler hitedStatusHandlers in statusHandlersOnNode)
        {
            foreach (StatusData statusData in actionToResolve.StatusToApply)
            {
                hitedStatusHandlers.ApplyStatus(statusData, actionToResolve.StatusDuration, resolvingSkill.Caster.HoldingEntity);
            }
        }
    }
}
