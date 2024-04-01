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

    public Action<AppliedStatus> onUpdateStatusDuration;

    public EC_StatusHandler StatusTarget => targetStatusHandler;

    public AppliedStatus(StatusData nStatus, float nDuration, Action endCallback)
    {
        status = nStatus;
        duration = RoundManager.Instance.CreateRoundTimer(nDuration, UpdateStatusDuration, EndStatus);

        endStatusCallback = endCallback;
    }

    public void ApplyStatus(EC_StatusHandler nStatusHandler, Entity nStatusOrigin)
    {
        targetStatusHandler = nStatusHandler;
        statusOrigin = nStatusOrigin;

        foreach (StatusEffect effect in status.Effects)
        {
            effect.ApplyEffect(this);
        }
    }

    public void RemoveStatus()
    {
        foreach (StatusEffect effect in status.Effects)
        {
            effect.RemoveEffect(this);
        }
    }

    public void ProgressStatusDuration()
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
        {
            duration.ProgressRound(1);

            UpdateStatusDuration();
        }
    }

    private void UpdateStatusDuration()
    {
        onUpdateStatusDuration?.Invoke(this);
    }

    private void EndStatus()
    {
        endStatusCallback?.Invoke();
    }
}
