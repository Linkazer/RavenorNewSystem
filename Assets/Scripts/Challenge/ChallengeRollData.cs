using ReferencePicker;
using System;
using UnityEngine;

/// <summary>
/// The data of a Challenge Roll
/// </summary>
[Serializable]
public class ChallengeRollData
{
    /// <summary>
    /// The Expertise used for the challenge.
    /// </summary>
    public ChallengeExpertise expertise;
    /// <summary>
    /// The Score dices must beat.
    /// </summary>
    public int difficulty;
}

