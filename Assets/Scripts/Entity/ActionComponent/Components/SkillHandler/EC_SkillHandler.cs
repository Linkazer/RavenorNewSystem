using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_SkillHandler : EntityActionComponent<IEC_SkillHandlerData>
{
    [SerializeField] private List<SkillHolder> usableSkills = new List<SkillHolder>();

    [SerializeField] private SkillHolder opportunitySkill;

    [SerializeField] private int offensiveAdvantage;
    [SerializeField] private int offensiveDisavantage;

    private SkillRessourceType ressourceTypeData;
    private SkillRessource ressourceUsed;
    private int opportunityAttackLeft = 1;

    private SKL_SkillScriptable selectedSkill = null;

    private EC_NodeHandler nodeHandler;

    private Dictionary<SkillComplexity, int> skillComplexityActionByAmountInTurn = new Dictionary<SkillComplexity, int>();
    private Dictionary<SkillComplexity, int> skillComplexityActionByAmountInTurnLeft = new Dictionary<SkillComplexity, int>();

    public int OffensiveAdvantage => offensiveAdvantage;
    public int OffensiveDisavantage => offensiveDisavantage;

    public Node CurrentNode => nodeHandler.CurrentNode;

    public List<SkillHolder> UsableSkills => usableSkills;

    public SKL_SkillScriptable SelectedSkill => selectedSkill;

    public SkillRessource RessourceUsed => ressourceUsed;
    public SkillRessourceType RessourceTypeData => ressourceTypeData;

    public override void SetComponentData(IEC_SkillHandlerData componentData)
    {
        offensiveAdvantage = componentData.OffensiveAdvantage;
        offensiveDisavantage = componentData.OffensiveDisavantage;
        ressourceTypeData = componentData.RessourceTypeUsed;

        if (ressourceTypeData != null)
        {
            ressourceUsed = ressourceTypeData.RessourceBehavior.GetAsNew();
        }
        else
        {
            ressourceUsed = null;
        }

        skillComplexityActionByAmountInTurn = new Dictionary<SkillComplexity, int> { { SkillComplexity.Ordinary, 1 }, { SkillComplexity.Fast, 1 } };
        skillComplexityActionByAmountInTurnLeft = new Dictionary<SkillComplexity, int>(skillComplexityActionByAmountInTurn);

        usableSkills = new List<SkillHolder>();

        foreach (SKL_SkillScriptable skill in componentData.Skills)
        {
            SkillHolder skillHolder = new SkillHolder(skill);
            usableSkills.Add(skillHolder);
        }
    }

    protected override void InitializeComponent()
    {
        if (!HoldingEntity.TryGetEntityComponentOfType<EC_NodeHandler>(out nodeHandler))
        {
            Debug.LogError(HoldingEntity + " has EC_SkillHandler without EC_NodeHandler.");
        }
        else
        {
            nodeHandler.actOnExitNode += RemoveOpportunityAttack;
            nodeHandler.actOnEnterNode += PlaceOpportunityAttack;
            PlaceOpportunityAttack(CurrentNode);
        }

        if(ressourceUsed != null)
        {
            ressourceUsed.Initialize(this);
        }
    }

    public override void Activate()
    {
        if (ressourceUsed != null)
        {
            ressourceUsed.Ativate();
        }
    }

    public override void Deactivate()
    {
        if (ressourceUsed != null)
        {
            ressourceUsed.Deactivate();
        }

        RemoveOpportunityAttack(CurrentNode);
    }

    public override void StartRound()
    {
        skillComplexityActionByAmountInTurnLeft = new Dictionary<SkillComplexity, int>(skillComplexityActionByAmountInTurn);
        opportunityAttackLeft = 1;
    }

    public override void EndRound()
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
        {
            foreach (SkillHolder skillHolder in usableSkills)
            {
                skillHolder.ProgressCooldown();
            }
        }
    }

    public override bool IsActionAvailable()
    {
        return true;
    }

    public bool CanSelectSkill(SkillHolder skillToCheck)
    {
        if(!skillToCheck.IsUsable())
        {
            return false;
        }

        if(ressourceUsed != null && !ressourceUsed.HasEnoughRessource(skillToCheck.Scriptable.RessourceCost))
        {
            return false;
        }
        
        if(!CheckUsabilityByComplexity(skillToCheck.Scriptable.CastComplexity))
        {
            return false;
        }

        return true;
    }

    private bool CheckUsabilityByComplexity(SkillComplexity complexityUsed)
    {
        switch (complexityUsed)
        {
            case SkillComplexity.Ordinary:
                return skillComplexityActionByAmountInTurnLeft[SkillComplexity.Ordinary] > 0;
            case SkillComplexity.Fast:
                return skillComplexityActionByAmountInTurnLeft[SkillComplexity.Fast] > 0 || skillComplexityActionByAmountInTurnLeft[SkillComplexity.Ordinary] > 0;
            case SkillComplexity.Instant:
                return true;
        }

        return true;
    }

    public void SelectSkill(SKL_SkillScriptable skillScriptableToSelect)
    {
        selectedSkill = skillScriptableToSelect;
    }

    protected override void OnUseAction(Vector3 actionTargetPosition)
    {
        if(selectedSkill != null && CanUseSkillAtNode(selectedSkill, CurrentNode, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition)))
        {
            ResolveSkill(selectedSkill, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition));

            selectedSkill = null;
        }
    }

    public void ResolveSkill(SKL_SkillScriptable skillData, Node resolutionPosition)
    {
        if (HoldingEntity.TryGetEntityComponentOfType(out EC_Renderer renderer))
        {
            renderer.SetOrientation(resolutionPosition.WorldPosition - holdingEntity.transform.position);
        }

        SKL_ResolvingSkillData resolvingSkillData = new SKL_ResolvingSkillData(skillData, this, resolutionPosition);
        SKL_SkillResolverManager.Instance.ResolveSpell(resolvingSkillData, OnEndResolveSkill);

        SpendActionByComplexiy(skillData.CastComplexity);
        GetSkillHolderForScriptable(selectedSkill)?.UseSkill();
    }

    private void SpendActionByComplexiy(SkillComplexity complexityUsed)
    {
        switch(complexityUsed)
        {
            case SkillComplexity.Ordinary:
                skillComplexityActionByAmountInTurnLeft[SkillComplexity.Ordinary]--;
                break;
            case SkillComplexity.Fast:
                if (skillComplexityActionByAmountInTurnLeft[SkillComplexity.Fast] > 0)
                {
                    skillComplexityActionByAmountInTurnLeft[SkillComplexity.Fast]--;
                }
                else
                {
                    skillComplexityActionByAmountInTurnLeft[SkillComplexity.Ordinary]--;
                }
                break;
            case SkillComplexity.Instant:
                //No Action used.
                break;
        }
    }

    public bool CanUseSkillAtNode(SKL_SkillScriptable skillToCheck, Node castNode, Node nodeToCheck)
    {
        return Pathfinding.Instance.IsNodeVisible(castNode, nodeToCheck, skillToCheck.Range);
    }

    public override void CancelAction()
    {
        
    }

    private void OnEndResolveSkill()
    {
        EndAction();
    }

    protected override void OnEndAction()
    {
        selectedSkill = null;
    }

    public SkillHolder GetSkillHolderForScriptable(SKL_SkillScriptable scriptable)
    {
        foreach(SkillHolder skill in usableSkills)
        {
            if(skill.Scriptable == scriptable)
            {
                return skill;
            }
        }

        return null;
    }

    private void PlaceOpportunityAttack(Node nodeToPlaceAround)
    {
        if (opportunitySkill != null && opportunitySkill.Scriptable != null)
        {
            List<Node> targetNodes = Pathfinding.Instance.GetAllNodeInDistance(nodeToPlaceAround, opportunitySkill.Scriptable.Range, true);

            foreach (Node node in targetNodes)
            {
                if (!node.exitBlockers.Contains(CheckOpportunityAttack))
                {
                    node.exitBlockers.Add(CheckOpportunityAttack);
                }
            }
        }
    }

    private void RemoveOpportunityAttack(Node nodeToRemoveAround)
    {
        if (opportunitySkill != null && opportunitySkill.Scriptable != null)
        {
            List<Node> targetNodes = Pathfinding.Instance.GetAllNodeInDistance(nodeToRemoveAround, opportunitySkill.Scriptable.Range, true);

            foreach (Node node in targetNodes)
            {
                if (node.exitBlockers.Contains(CheckOpportunityAttack))
                {
                    node.exitBlockers.Remove(CheckOpportunityAttack);
                }
            }
        }
    }

    private bool CheckOpportunityAttack(EC_NodeHandler attackTarget, Action callback, object[] triggerData)
    {
        bool doesTriggerAttack = (bool)triggerData[0];

        if(opportunitySkill != null)
        {
            if(opportunityAttackLeft > 0 && HoldingEntity.IsHostileTo(attackTarget.HoldingEntity))
            {
                if (doesTriggerAttack)
                {
                    opportunityAttackLeft--;
                    
                    endActionCallback += callback;

                    ResolveSkill(opportunitySkill.Scriptable, attackTarget.CurrentNode);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
