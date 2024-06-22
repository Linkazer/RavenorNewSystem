using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterHostility
{
    Ally,
    Hostile,
    Neutral
}

public class CharacterEntity : Entity
{
    [SerializeField] private CharacterScriptable characterData;

    [SerializeField] private int priority;

    public CharacterScriptable CharacterData => characterData;

    public int Priority => priority;

    public override IEntityData GetComponentData()
    {
        return characterData;
    }

    public override void Activate()
    {
        base.Activate();

        CharacterManager.Instance.AddActiveCharacter(this);
    }

    public override void Deactivate()
    {
        CharacterManager.Instance.RemoveActiveCharacter(this);

        base.Deactivate();
    }
}
