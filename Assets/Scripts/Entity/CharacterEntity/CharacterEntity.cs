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

    [SerializeField] private CharacterHostility hostility;
    [SerializeField] private int teamIndex = 0;

    [SerializeField] private int priority;

    public CharacterScriptable CharacterData => characterData;

    public CharacterHostility Hostility => hostility;

    public int TeamIndex => teamIndex;

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

    public void SetHostile(CharacterHostility toSet)
    {
        if(hostility != toSet)
        {
            hostility = toSet;

            CharacterManager.Instance.UpdateCharacterHostility(this);
        }
    }
}
