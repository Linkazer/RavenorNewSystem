using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AI Character", menuName = "Character/New AI Character")]
public class AICharacterScriptable : CharacterScriptable
{
    [SerializeField] private AIConcideration[] conciderations;

    public AIConcideration[] Condiderations => conciderations;
}
