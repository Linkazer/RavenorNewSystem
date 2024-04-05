using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_StatusHandler : EntityComponent<IEC_StatusHandlerData>
{
    [SerializeField] private List<StatusData> passives = new List<StatusData>();

    [SerializeField] private Transform effectVisualHolder;

    private Dictionary<StatusData, AppliedStatus> effectiveStatus = new Dictionary<StatusData, AppliedStatus>();

    public override void SetComponentData(IEC_StatusHandlerData componentData)
    {
        foreach(StatusData passive in componentData.Passives)
        {
            passives.Add(passive);
        }
    }

    protected override void InitializeComponent()
    {
        foreach(StatusData passive in passives)
        {
            ApplyStatus(passive, 2f, holdingEntity);
        }
    }

    public override void Activate()
    {
        
    }

    public override void Deactivate()
    {
        
    }

    public override void StartRound()
    {

    }

    public override void EndRound()
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
        {
            foreach (KeyValuePair<StatusData, AppliedStatus> status in effectiveStatus)
            {
                status.Value.ProgressStatusDuration();
            }
        }
    }

    public void ApplyStatus(StatusData status, float duration, Entity statusOrigin)
    {
        if (effectiveStatus.ContainsKey(status))
        {
            //TODO : Stack + Reset Cooldown et autres
        }
        else
        {
            AppliedStatus appliedEffect = new AppliedStatus(status, duration, () => RemoveStatus(status));

            appliedEffect.ApplyStatus(this, statusOrigin);

            if (status.StatusFX != null)
            {
                appliedEffect.statusFX = PoolManager.InstatiatePoolableAtPosition(status.StatusFX, effectVisualHolder.position);
                appliedEffect.statusFX.transform.parent = effectVisualHolder;
            }

            effectiveStatus.Add(status, appliedEffect);
        }
    }

    public void RemoveStatus(StatusData status)
    {
        if(effectiveStatus.ContainsKey(status))
        {
            if (effectiveStatus[status].statusFX != null)
            {
                effectiveStatus[status].statusFX.End();
            }
            effectiveStatus[status].RemoveStatus();
            effectiveStatus.Remove(status);
        }
    }
}
