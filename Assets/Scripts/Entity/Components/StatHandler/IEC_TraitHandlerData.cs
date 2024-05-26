using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChallengeExpertiseLevel
{
    public int level;
    public ChallengeExpertise expertise;
}

public interface IEC_TraitHandlerData : IEntityData
{
    public int Force { get; }
    public int Esprit { get; }
    public int Presence { get; }
    public int Agilite { get; }
    public int Instinct { get; }

    public List<ChallengeExpertiseLevel> Expertises { get; }
}
