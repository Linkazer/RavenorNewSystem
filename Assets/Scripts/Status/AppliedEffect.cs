using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppliedEffect
{
    private StatusEffect effect;
    private AppliedStatus effectStatus;

    public StatusEffect Effect => effect;
    public AppliedStatus EffectStatus => effectStatus;

    public AppliedEffect(StatusEffect effectToApply, AppliedStatus parentStatus)
    {
        effect = effectToApply;
        effectStatus = parentStatus;
    }

    public void ApplyEffect()
    {
        SetTrigger();
    }

    public void RemoveEffect()
    {
        UnsetTrigger();
    }

    private void SetTrigger()
    {
        switch (effect.Trigger)
        {
            case StatusTrigger.OnApply:
                DoEffect();
                break;
            case StatusTrigger.OnStatusEnd:
                //Nothing : Done in UnsetTrigger
                break;
            case StatusTrigger.OnUpdateStatusTurn:
                effectStatus.onUpdateStatusDuration += DoEffect;
                break;
            case StatusTrigger.OnHolderBeginTurn:
                effectStatus.StatusTarget.HoldingEntity.actOnEntityStartRound += DoEffect;
                break;
            case StatusTrigger.OnHolderEndTurn:
                effectStatus.StatusTarget.HoldingEntity.actOnEntityEndRound += DoEffect;
                break;
        }
    }

    private void UnsetTrigger()
    {
        switch (effect.Trigger)
        {
            case StatusTrigger.OnApply:
                UndoEffect();
                break;
            case StatusTrigger.OnStatusEnd:
                DoEffect();
                break;
            case StatusTrigger.OnUpdateStatusTurn:
                effectStatus.onUpdateStatusDuration -= DoEffect;
                break;
            case StatusTrigger.OnHolderBeginTurn:
                effectStatus.StatusTarget.HoldingEntity.actOnEntityStartRound -= DoEffect;
                break;
            case StatusTrigger.OnHolderEndTurn:
                effectStatus.StatusTarget.HoldingEntity.actOnEntityEndRound -= DoEffect;
                break;
        }
    }

    private void DoEffect()
    {
        effect.DoEffect(effectStatus);
    }

    private void UndoEffect()
    {
        effect.UndoEffect(effectStatus);
    }
}
