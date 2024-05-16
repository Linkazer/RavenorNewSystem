using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AS_RollDice : SKL_SkillAction
{
    [SerializeField] private int numberDicesToRoll;
    [SerializeField] private EntityTraits offensiveTrait;
    [SerializeField] private SkillDefensiveTrait defensiveTrait;

    [Header("Animation")]
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_SkillActionAnimation))] private SKL_SkillActionAnimation[] onDiceRollAnimation;

    [Header("Actions")]
    [SerializeField] private SKL_SkillActionChooser touchAction;
    [SerializeField] private SKL_SkillActionChooser touchSubActionOnTarget;
    [SerializeField] private SKL_SkillActionChooser noTouchAction;
    [SerializeField] private SKL_SkillActionChooser noTouchSubActionOnTarget;
    [SerializeField] private SKL_SkillActionChooser nextAction;

    public int NumberDicesRoll => numberDicesToRoll;

    public EntityTraits OffensiveTrait => offensiveTrait;
    public SkillDefensiveTrait DefensiveTrait => defensiveTrait;

    public SKL_SkillActionAnimation[] OnDiceRollAnimation => onDiceRollAnimation;

    public SKL_SkillAction GetTouchAction(SKL_ResolvingSkillData resolvingSkillData)
    {
        return touchAction.GetFirstUsableAction(resolvingSkillData);
    }
    
    public SKL_SkillAction GetTouchActionOnTarget(SKL_ResolvingSkillData resolvingSkillData)
    {
        return touchSubActionOnTarget.GetFirstUsableAction(resolvingSkillData);
    }

    public SKL_SkillAction GetNoTouchAction(SKL_ResolvingSkillData resolvingSkillData)
    {
        return noTouchAction.GetFirstUsableAction(resolvingSkillData);
    }
    
    public SKL_SkillAction GetNoTouchActionOnTarget(SKL_ResolvingSkillData resolvingSkillData)
    {
        return noTouchSubActionOnTarget.GetFirstUsableAction(resolvingSkillData);
    }

    public SKL_SkillAction GetNextAction(SKL_ResolvingSkillData resolvingSkillData)
    {
        return nextAction.GetFirstUsableAction(resolvingSkillData);
    }
}