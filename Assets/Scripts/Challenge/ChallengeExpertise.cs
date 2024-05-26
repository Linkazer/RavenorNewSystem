using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Expertise", menuName = "Challenge/Expertise")]
public class ChallengeExpertise: ScriptableObject
{
    [SerializeField] private RVN_Text expertiseTitle;
    [SerializeField] private EntityTraits trait;

    public string Title => expertiseTitle.GetText();

    public EntityTraits Trait => trait;
}
