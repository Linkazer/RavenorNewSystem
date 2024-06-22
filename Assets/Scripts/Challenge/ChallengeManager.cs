using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Manager that handle Challenge Rolls.
/// </summary>
public class ChallengeManager : Singleton<ChallengeManager>
{
    [SerializeField] private UI_ChallengeRoll ui;

    /// <summary>
    /// Initialize a Challenge.
    /// </summary>
    /// <param name="challengeRoll">The data of the Challenge Roll.</param>
    /// <param name="challengedHandler">The TraitHandler that is challenged.</param>
    /// <param name="callback">The Action called at the end of the challenge, with the amount of succeed dices.</param>
    public void InitializeChallenge(ChallengeRollData challengeRoll, EC_TraitHandler challengedHandler, Action<int> callback)
    {
        ui.InitializeChallenge(challengeRoll, challengedHandler, callback);
    }
}
