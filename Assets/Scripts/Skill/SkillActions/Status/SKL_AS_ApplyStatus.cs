using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AS_ApplyStatus : SKL_SkillAction
{
    [Header("Status")]
    [SerializeField] private float statusDuration;
    [SerializeField] private StatusData[] statusToApply;

    [Header("Animation")]
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_SkillActionAnimation))] private SKL_SkillActionAnimation[] applyStatusAnimations;

    [Header("Actions")]
    [SerializeField] private SKL_SkillActionChooser nextAction;

    public float StatusDuration => statusDuration;
    public StatusData[] StatusToApply => statusToApply;

    public SKL_SkillActionAnimation[] ApplyStatusAnimations => applyStatusAnimations;

    public SKL_SkillAction GetNextAction(SKL_ResolvingSkillData resolvingSkillData)
    {
        return nextAction.GetFirstUsableAction(resolvingSkillData);
    }
}
