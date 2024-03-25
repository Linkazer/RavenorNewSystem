using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReferencePicker;

[Serializable]
public abstract class SKL_SkillAction
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_SkillActionShape))] protected SKL_SkillActionShape shape = new SKL_Shape_TargetNode();
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_SkillActionAnimation))] protected SKL_SkillActionAnimation[] startAnimations = null;

    public SKL_SkillActionShape Shape => shape;

    public SKL_SkillActionAnimation[] StartAnimations => startAnimations;
}
