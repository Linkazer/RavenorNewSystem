using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppliedStatus
{
    private StatusData status;
    private RoundTimer duration;
    private EC_StatusHandler targetStatusHandler;
    private Entity statusOrigin;

    private Action endStatusCallback;

    private List<AppliedEffect> effects = new List<AppliedEffect>();

    public Action onUpdateStatusDuration;

    public Poolable_FX statusFX;

    public EC_StatusHandler StatusTarget => targetStatusHandler;

    public StatusData Status;

    public AppliedStatus(StatusData nStatus, float nDuration, Action endCallback)
    {
        status = MonoBehaviour.Instantiate(nStatus);
        duration = RoundManager.Instance.CreateRoundTimer(nDuration, UpdateStatusDuration, EndStatus);

        endStatusCallback = endCallback;
    }

    public void ApplyStatus(EC_StatusHandler nStatusHandler, Entity nStatusOrigin)
    {
        targetStatusHandler = nStatusHandler;
        statusOrigin = nStatusOrigin;

        foreach (StatusEffect effect in status.Effects)
        {
            AppliedEffect appliedEffect = new AppliedEffect(effect, this);
            appliedEffect.ApplyEffect();
            effects.Add(appliedEffect);
        }
    }

    public void RemoveStatus()
    {
        foreach(AppliedEffect appliedEffect in effects)
        {
            appliedEffect.RemoveEffect();
        }

        effects.Clear();
    }

    public void ResetStatusDuration()
    {
        duration?.StopRealTimer();
        duration = RoundManager.Instance.CreateRoundTimer(duration.maximumRound, UpdateStatusDuration, EndStatus);
    }

    public void ProgressStatusDuration()
    {
        duration.ProgressRound(1);

        UpdateStatusDuration();
    }

    private void UpdateStatusDuration()
    {
        onUpdateStatusDuration?.Invoke();
    }

    private void EndStatus()
    {
        endStatusCallback?.Invoke();
    }
}
