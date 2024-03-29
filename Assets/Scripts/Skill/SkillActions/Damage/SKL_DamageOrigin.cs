using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SKL_DamageOrigin
{
    public abstract int GetDamageAmount(SKL_ResolvingSkillData damageData);
}

public class SKL_DO_Dices : SKL_DamageOrigin
{
    public override int GetDamageAmount(SKL_ResolvingSkillData damageData)
    {
        int toReturn = 0;

        foreach(Dice dice in damageData.dicesResult)
        {
            if(dice.DoesHit)
            {
                toReturn++;
            }
        }

        return toReturn;
    }
}

public class SKL_DO_Direct : SKL_DamageOrigin
{
    [SerializeField] private int damageAmount;

    public override int GetDamageAmount(SKL_ResolvingSkillData damageData)
    {
        return damageAmount;
    }
}

/*public class SPL_DO_EffectStack : SPL_DamageOrigin
{
    [SerializeField] private EffectScriptable effectToScaleOn;
    [SerializeField] private int damageByStack;

    public override int GetDamageAmount(SPL_DamageActionData damageData)
    {
        if(damageData.target.Handler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler targetEffectHandler))
        {
            return targetEffectHandler.GetEffectStack(effectToScaleOn) * damageByStack;
        }
        else
        {
            return 0;
        }
    }
}*/