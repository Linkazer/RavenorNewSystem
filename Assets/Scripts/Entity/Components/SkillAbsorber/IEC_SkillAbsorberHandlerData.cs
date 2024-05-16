using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_SkillAbsorberHandlerData : IEntityData
{
    public int DodgeBonus { get; }
    public int WillBonus { get; }
    public int DefensiveAdvantage { get; }
    public int DefensiveDisavantage { get; }
}
