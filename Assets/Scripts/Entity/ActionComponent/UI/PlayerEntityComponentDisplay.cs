using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEntityComponentDisplay : MonoBehaviour
{
    protected CharacterEntity characterEntity;

    public abstract void SetCharacter(CharacterEntity characterToSet);
}

public abstract class PlayerEntityComponentDisplay<T> : PlayerEntityComponentDisplay where T : EntityComponent
{
    protected T componentHandled;

    public override void SetCharacter(CharacterEntity characterToSet)
    {
        if (componentHandled != null)
        {
            UnsetComponent(componentHandled);
        }

        componentHandled = null;

        characterEntity = characterToSet;

        if (characterEntity != null)
        {
            if (characterEntity.TryGetEntityComponentOfType(out T foundComponent))
            {
                componentHandled = foundComponent;
                SetComponent(componentHandled);
            }
        }
    }

    protected abstract void SetComponent(T entityComponent);

    protected abstract void UnsetComponent(T entityComponent);
}
