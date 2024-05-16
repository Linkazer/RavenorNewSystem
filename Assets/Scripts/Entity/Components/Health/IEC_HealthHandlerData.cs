using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_HealthHandlerData : IEntityData
{
    public int MaxHealth { get; }
    public int CurrentHealth { get; }
    public int MaxArmor { get; }
    public int CurrentArmor { get; }
    public float UiHeight { get; }
}
