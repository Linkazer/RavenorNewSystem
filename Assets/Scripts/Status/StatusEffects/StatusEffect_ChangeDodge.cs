using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_ChangeDodge : StatusEffect
{
    [SerializeField] private int dodgeToGain;
    
    public override void DoEffect(AppliedStatus appliedStatus)
    {
        if (appliedStatus.StatusTarget.HoldingEntity.TryGetEntityComponentOfType(out EC_HealthHandler healthHandler))
        {
            healthHandler.AddDodgeBonus(dodgeToGain);
        }
    }

    public override void UndoEffect(AppliedStatus appliedStatus)
    {
        if (appliedStatus.StatusTarget.HoldingEntity.TryGetEntityComponentOfType(out EC_HealthHandler healthHandler))
        {
            healthHandler.RemoveDodgeBonus(dodgeToGain);
        }
    }
}
