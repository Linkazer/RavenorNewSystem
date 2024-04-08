using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : Entity
{
    [SerializeField] private CharacterScriptable characterData;

    [SerializeField] private bool isHostile = false;
    [SerializeField] private int teamIndex = 0;

    public CharacterScriptable CharacterData => characterData;

    public bool IsHostile => isHostile;

    public int TeamIndex => teamIndex;

    public override IEntityData GetComponentData()
    {
        return characterData;
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        SetHostile(false);

        base.Deactivate();
    }

    public void SetHostile(bool toSet)
    {
        if(isHostile != toSet)
        {
            isHostile = toSet;

            if(isHostile)
            {
                BattleRoundManager.Instance.StartBattle();
            }
        }
    }
}
