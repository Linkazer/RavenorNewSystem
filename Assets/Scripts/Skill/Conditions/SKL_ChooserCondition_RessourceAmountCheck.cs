using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_ChooserCondition_RessourceAmountCheck : SKL_SkillActionChooserCondition
{
    private enum CheckToDo
    {
        Less,
        LessOrEqual,
        Equal,
        MoreOrEqual,
        More,
        NotEqual
    }

    [SerializeField] private int ressourceAmountToCheck;
    [SerializeField] private CheckToDo checkToDo;

    public override bool IsConditionValid(SKL_ResolvingSkillData castedSpellData)
    {
        if (castedSpellData.Caster.RessourceUsed != null)
        {
            int currentCasterRessource = castedSpellData.Caster.RessourceUsed.CurrentAmount;

            switch (checkToDo)
            {
                case CheckToDo.Less:
                    return currentCasterRessource < ressourceAmountToCheck;
                case CheckToDo.LessOrEqual:
                    return currentCasterRessource <= ressourceAmountToCheck;
                case CheckToDo.Equal:
                    return currentCasterRessource == ressourceAmountToCheck;
                case CheckToDo.MoreOrEqual:
                    return currentCasterRessource >= ressourceAmountToCheck;
                case CheckToDo.More:
                    return currentCasterRessource > ressourceAmountToCheck;
                case CheckToDo.NotEqual:
                    return currentCasterRessource != ressourceAmountToCheck;
            }
        }

        return false;
    }
}
