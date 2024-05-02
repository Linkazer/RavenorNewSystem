using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AS_RemoveRessource : SKL_SkillAction
{
    [SerializeField] private SkillRessourceType ressourceType;
    [SerializeField] private int amountToRemove;

    [Header("Actions")]
    [SerializeField] private SKL_SkillActionChooser nextAction;

    public SkillRessourceType RessourceType => ressourceType;

    public int AmountToRemove => amountToRemove;

    public SKL_SkillAction GetNextAction(SKL_ResolvingSkillData resolvingSkillData)
    {
        return nextAction.GetFirstUsableAction(resolvingSkillData);
    }
}
