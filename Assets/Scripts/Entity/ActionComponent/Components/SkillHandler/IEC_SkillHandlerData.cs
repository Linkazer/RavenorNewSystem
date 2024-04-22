using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_SkillHandlerData : IEntityData
{
    public List<SKL_SkillScriptable> Skills { get; }

    public int Force { get; }
    public int Esprit { get; }
    public int Presence { get; }
    public int Agilite { get; }
    public int Instinct { get; }

    public int OffensiveAdvantage { get; }
    public int OffensiveDisavantage { get; }

    public SkillRessourceType RessourceTypeUsed { get; }
}
