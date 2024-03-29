using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AS_HealAction : SKL_SkillAction
{
    [Header("Damage Datas")]
    [SerializeField] private SKL_DamageData[] healsData;

    [Header("Animation")]
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_SkillActionAnimation))] private SKL_SkillActionAnimation[] healAnimations;

    [Header("Actions")]
    [SerializeField] private SKL_SkillActionChooser nextAction;

    public SKL_DamageData[] HealsData => healsData;

    public SKL_SkillActionAnimation[] HealAnimation => healAnimations;

    public SKL_SkillAction GetNextAction(SKL_ResolvingSkillData resolvingSkillData)
    {
        return nextAction.GetFirstUsableAction(resolvingSkillData);
    }
}
