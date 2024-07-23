using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIConcideration
{
    [SerializeField] private SKL_SkillScriptable skillToCheck;
    [SerializeField] private AICalcul_Conditional[] conditions;
    [SerializeField] private float bonusScore;
    [SerializeField, SerializeReference, ReferenceEditor(typeof(AICalcul))] private AICalcul[] scoreCalculs;

    public SKL_SkillScriptable SkillToCheck => skillToCheck;
    public AICalcul_Conditional[] Conditions => conditions;
    public float BonusScore => bonusScore;
    public AICalcul[] ScoreCalculs => scoreCalculs;
}