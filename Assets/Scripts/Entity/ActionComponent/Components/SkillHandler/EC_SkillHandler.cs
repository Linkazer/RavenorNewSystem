using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_SkillHandler : EntityActionComponent<IEC_SkillHandlerData>
{
    [SerializeField] private List<SkillHolder> usableSkills = new List<SkillHolder>();

    [SerializeField] private int accuracy;
    [SerializeField] private int physicalPower;
    [SerializeField] private int magicalPower;  
    [SerializeField] private int offensiveAdvantage;
    [SerializeField] private int offensiveDisavantage;

    private SkillHolder selectedSkill = null;

    private EC_NodeHandler nodeHandler;

    public int Accuracy => accuracy;
    public int PhysicalPower => physicalPower;
    public int MagicalPower => magicalPower;
    public int OffensiveAdvantage => offensiveAdvantage;
    public int OffensiveDisavantage => offensiveDisavantage;

    public Node CurrentNode => nodeHandler.CurrentNode;

    public List<SkillHolder> UsableSkills => usableSkills;

    public SkillHolder SelectedSkill => selectedSkill;

    public override void SetComponentData(IEC_SkillHandlerData componentData)
    {
        accuracy = componentData.Accuracy;
        physicalPower = componentData.PhysicalPower;
        magicalPower = componentData.MagicalPower;
        offensiveAdvantage = componentData.OffensiveAdvantage;
        offensiveDisavantage = componentData.OffensiveDisavantage;

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
    }

    public override void Activate()
    {
        
    }

    public override void Deactivate()
    {
        
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

    public void SelectSkill(SkillHolder skillHolderToSelect)
    {
        selectedSkill = skillHolderToSelect;
    }

    protected override void OnUseAction(Vector3 actionTargetPosition)
    {
        if(selectedSkill != null)
        {
            SKL_ResolvingSkillData resolvingSkillData = new SKL_ResolvingSkillData(selectedSkill.Scriptable, this, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition));
            SKL_SkillResolverManager.Instance.ResolveSpell(resolvingSkillData, OnEndResolveSkill);
            selectedSkill.UseSkill();
        }
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
}
