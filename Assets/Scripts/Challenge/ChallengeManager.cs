using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : Singleton<ChallengeManager>
{
    [SerializeField] private UI_ChallengeRoll ui;

    public void InitializeChallenge(ChallengeRollData challengeRoll, EC_TraitHandler challengedHandler, Action<int> callback)
    {
        ui.InitializeChallenge(challengeRoll, challengedHandler, callback);
    }
}
