using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SKL_RSC_Chanting : SkillRessource
{
    public override SkillRessource GetAsNew()
    {
        return new SKL_RSC_Chanting();
    }

    public override void Initialize(EC_SkillHandler nSkillHandler)
    {
        limits = new Vector2Int(0, 3);
        startAmount = 0;

        base.Initialize(nSkillHandler);
    }

    public override bool HasEnoughRessource(int amountNeeded)
    {
        return true;
    }

    public override void OnUseSkillWithRessource(int ressourceAmount)
    {
        AddRessource(1);
    }
}
