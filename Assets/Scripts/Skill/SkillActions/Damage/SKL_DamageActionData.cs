using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_DamageActionData
{
    public EC_SkillHandler caster;
    public EC_HealthHandler target;
    public List<Dice> dices;
    public bool didHit;
    public List<KeyValuePair<SKL_DamageType, int>> previousDamageResults;

    public SKL_DamageActionData(EC_SkillHandler nCaster, EC_HealthHandler nTarget, List<Dice> nDices, bool nDidHit, List<KeyValuePair<SKL_DamageType, int>> nPreviousDamageResults)
    {
        caster = nCaster;
        target = nTarget;
        dices = nDices;
        didHit = nDidHit;
        previousDamageResults = nPreviousDamageResults;
    }
}
