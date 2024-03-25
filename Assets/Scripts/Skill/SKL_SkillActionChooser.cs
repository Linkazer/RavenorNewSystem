using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SKL_SkillActionChooser
{
    [Serializable]
    private class ActionByCondition
    {
        [SerializeReference, ReferenceEditor(typeof(SKL_SkillActionChooserCondition))] public SKL_SkillActionChooserCondition[] conditions = null;
        [SerializeReference, ReferenceEditor(typeof(SKL_SkillAction))] public SKL_SkillAction spellAction = null;

        public bool IsUsable(SKL_ResolvingSkillData castedSpellData)
        {
            foreach (SKL_SkillActionChooserCondition condition in conditions)
            {
                if (!condition.IsConditionValid(castedSpellData))
                {
                    return false;
                }
            }

            return true;
        }
    }

    [SerializeField] private ActionByCondition[] possibleActions;

    public SKL_SkillAction GetFirstUsableAction(SKL_ResolvingSkillData castedSpellData)
    {
        foreach (ActionByCondition actionByCondition in possibleActions)
        {
            if (actionByCondition.IsUsable(castedSpellData))
            {
                return actionByCondition.spellAction;
            }
        }

        return null;
    }
}
