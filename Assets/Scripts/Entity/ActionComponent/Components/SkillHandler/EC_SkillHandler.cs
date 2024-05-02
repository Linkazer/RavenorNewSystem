using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillTrait
{
    Force,
    Esprit,
    Presence,
    Agilite,
    Instinct
}

public class EC_SkillHandler : EntityActionComponent<IEC_SkillHandlerData>
{
    [SerializeField] private List<SkillHolder> usableSkills = new List<SkillHolder>();

    [SerializeField] private int force;
    [SerializeField] private int esprit;
    [SerializeField] private int presence;
    [SerializeField] private int agilite;
    [SerializeField] private int instinct;
    [SerializeField] private int offensiveAdvantage;
    [SerializeField] private int offensiveDisavantage;

    private SkillRessource ressourceUsed;

    private SKL_SkillScriptable selectedSkill = null;

    private EC_NodeHandler nodeHandler;

    public int Force => force;
    public int Esprit => esprit;
    public int Presence => presence;
    public int Agilite => agilite;
    public int Instinct => instinct;
    public int OffensiveAdvantage => offensiveAdvantage;
    public int OffensiveDisavantage => offensiveDisavantage;

    public Node CurrentNode => nodeHandler.CurrentNode;

    public List<SkillHolder> UsableSkills => usableSkills;

    public SKL_SkillScriptable SelectedSkill => selectedSkill;

    public SkillRessource RessourceUsed => ressourceUsed;

    public override void SetComponentData(IEC_SkillHandlerData componentData)
    {
        force = componentData.Force;
        agilite = componentData.Agilite;
        instinct = componentData.Instinct;
        esprit = componentData.Esprit;
        presence = componentData.Presence;
        offensiveAdvantage = componentData.OffensiveAdvantage;
        offensiveDisavantage = componentData.OffensiveDisavantage;

        ressourceUsed = SkillRessourceHelper.GetNewRessourceFromEnum(componentData.RessourceTypeUsed);

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
    }

    public override void StartRound()
    {
        
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

    public void SelectSkill(SKL_SkillScriptable skillScriptableToSelect)
    {
        selectedSkill = skillScriptableToSelect;
    }

    protected override void OnUseAction(Vector3 actionTargetPosition)
    {
        if(selectedSkill != null && CanUseSkillAtNode(selectedSkill, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition)))
        {
            if(HoldingEntity.TryGetEntityComponentOfType(out EC_Renderer renderer))
            {
                renderer.SetOrientation(actionTargetPosition - holdingEntity.transform.position);
            }

            ResolveSkill(selectedSkill, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition));
            GetSkillHolderForScriptable(selectedSkill)?.UseSkill();

            selectedSkill = null;
        }
    }

    public void ResolveSkill(SKL_SkillScriptable skillData, Node resolutionPosition)
    {
        SKL_ResolvingSkillData resolvingSkillData = new SKL_ResolvingSkillData(skillData, this, resolutionPosition);
        SKL_SkillResolverManager.Instance.ResolveSpell(resolvingSkillData, OnEndResolveSkill);
    }

    private bool CanUseSkillAtNode(SKL_SkillScriptable skillToCheck, Node nodeToCheck)
    {
        return Pathfinding.Instance.IsNodeVisible(CurrentNode, nodeToCheck, skillToCheck.Range);
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

    private SkillHolder GetSkillHolderForScriptable(SKL_SkillScriptable scriptable)
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
}
