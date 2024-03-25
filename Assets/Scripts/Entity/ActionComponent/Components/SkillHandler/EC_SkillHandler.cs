using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_SkillHandler : EntityActionComponent<IEC_SkillHandlerData>
{
    public Node CurrentNode;

    private List<SkillHolder> usableSkills = new List<SkillHolder>();

    private IEC_SkillHandlerData data;

    public IEC_SkillHandlerData Data => data;

    public override void SetComponentData(IEC_SkillHandlerData componentData)
    {
        
    }

    protected override void InitializeComponent()
    {
        
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
        //TODO Update Cooldowns
    }

    public override bool IsActionAvailable()
    {
        return true;
    }

    public override bool IsActionUsable(Vector3 positionToCheck)
    {
        return true;
    }

    public override void SelectAction()
    {
        
    }

    public override void UnselectAction()
    {
        
    }

    protected override void OnUseAction(Vector3 actionTargetPosition)
    {
        
    }

    public override void CancelAction()
    {
        
    }

    protected override void OnEndAction()
    {
        
    }
}
