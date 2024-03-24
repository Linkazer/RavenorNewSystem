using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_SkillHandlerData : IEntityData
{
    //TODO Skills

    public int Precision { get; set; }
    public int PhysicalPower {  get; set; }
    public int MagicalPower {  get; set; }
    public int OffensiveAdvantage { get; set; }
    public int OffensiveDisavantage { get; set; }

    //TODO Ressource
}
