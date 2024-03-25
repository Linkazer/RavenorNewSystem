using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_AS_DamageAction : SKL_SkillAction
{
    [Header("Damage Datas")]
    [SerializeField] private int dicesUsed;
    [SerializeField] private SKL_DamageData[] damageData;

    [Header("Animation")]
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_SkillActionAnimation))] private SKL_SkillActionAnimation[] damageAnimations;

    [Header("Actions")]
    [SerializeField] private SKL_SkillActionChooser touchAction;
    [SerializeField] private SKL_SkillActionChooser noTouchAction;
    [SerializeField] private SKL_SkillActionChooser nextAction;
}
