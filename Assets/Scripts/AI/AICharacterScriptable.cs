using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AI Character", menuName = "Character/New AI Character")]
public class AICharacterScriptable : CharacterScriptable
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(AIMovementBehavior))] private AIMovementBehavior movementBehavior;

    [SerializeField] private AIConcideration[] conciderations;

    public AIMovementBehavior MovementBehavior => movementBehavior;

    public AIConcideration[] Condiderations => conciderations;
}
