using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SKL_SkillActionChooserCondition
{
    public abstract bool IsConditionValid(SKL_ResolvingSkillData castedSpellData);
}
