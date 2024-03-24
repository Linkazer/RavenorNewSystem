using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_HealthHandlerData : IEntityData
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxArmor { get; set; }
    public int CurrentArmor { get; set; }
    public int Dodge { get; set; }
    public int DefensiveAdvantage { get; set; }
    public int DefensiveDisavantage { get; set; }
}
