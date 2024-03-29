using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : Entity
{
    [SerializeField] private CharacterScriptable characterData;

    public override IEntityData GetComponentData()
    {
        return characterData;
    }
}
