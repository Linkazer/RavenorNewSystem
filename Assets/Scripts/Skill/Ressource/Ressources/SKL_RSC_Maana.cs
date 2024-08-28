using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SKL_RSC_Maana : SkillRessource
{
    public override SkillRessource GetAsNew()
    {
        return new SKL_RSC_Maana();
    }

    public override void Initialize(EC_SkillHandler nSkillHandler)
    {
        limits = new Vector2Int(0, 3);
        startAmount = 0;

        base.Initialize(nSkillHandler);
    }

    public override void Ativate()
    {
        base.Ativate();

        skillHandler.HoldingEntity.actOnEntityStartRound += OnHolderBeginTurn;
    }

    public override void Deactivate()
    {
        skillHandler.HoldingEntity.actOnEntityStartRound -= OnHolderBeginTurn;

        base.Deactivate();
    }

    private void OnHolderBeginTurn()
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
        {
            AddRessource(1);
        }
    }

    public override bool HasEnoughRessource(int amountNeeded)
    {
        return currentAmount >= amountNeeded;
    }

    public override void OnUseSkillWithRessource(int ressourceAmount)
    {
        RemoveRessource(ressourceAmount);
    }
}
