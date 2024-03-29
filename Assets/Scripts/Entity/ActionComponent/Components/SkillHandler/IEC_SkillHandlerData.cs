using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_SkillHandlerData : IEntityData
{
    public List<SKL_SkillScriptable> Skills { get; }

    public int Accuracy { get; }
    public int PhysicalPower {  get; }
    public int MagicalPower {  get; }
    public int OffensiveAdvantage { get; }
    public int OffensiveDisavantage { get; }

    //TODO Ressource
}
