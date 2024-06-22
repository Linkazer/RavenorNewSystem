using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Expertise that can be used for a Challenge Roll.
/// </summary>
[CreateAssetMenu(fileName = "Expertise", menuName = "Challenge/Expertise")]
public class ChallengeExpertise: ScriptableObject
{
    [SerializeField] private RVN_Text expertiseTitle;
    [SerializeField] private EntityTraits trait;

    public string Title => expertiseTitle.GetText();

    public EntityTraits Trait => trait;
}
