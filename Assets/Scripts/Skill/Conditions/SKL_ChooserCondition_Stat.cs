using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_ChooserCondition_Stat : SKL_SkillActionChooserCondition
{
    public override bool IsConditionValid(SKL_ResolvingSkillData castedSpellData)
    {
        return true;
    }
}
