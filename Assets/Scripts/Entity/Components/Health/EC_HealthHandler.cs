using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_HealthHandler : EntityComponent<IEC_HealthHandlerData>
{
    [SerializeField] private ECUI_HealthDisplayer healthDisplayer;

    private IEC_HealthHandlerData data;



    public override void SetComponentData(IEC_HealthHandlerData componentData)
    {
        data = componentData;
    }

    protected override void InitializeComponent()
    {
        data.CurrentHealth = data.MaxHealth;
        data.CurrentArmor = data.MaxArmor;
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
        
    }

    public void SetMaxHealth(int toSet)
    {
        data.MaxHealth = toSet;

        healthDisplayer.OnSetMaxHealth(data.MaxHealth, data.CurrentHealth);

        if (data.CurrentHealth > data.MaxHealth)
        {
            SetHealth(data.MaxHealth);
        }
    }

    public void GainHealth(int toGain)
    {
        healthDisplayer.OnGainHealth(toGain);

        if(data.CurrentHealth + toGain > data.MaxHealth)
        {
            SetHealth(data.MaxHealth);
        }
        else
        {
            SetHealth(data.CurrentHealth + toGain);
        }
    }

    public void LoseHealth(int toLose)
    {
        healthDisplayer.OnLoseHealth(toLose);

        if (data.CurrentHealth - toLose <= 0)
        {
            SetHealth(0);
            Succomb();
        }
        else
        {
            SetHealth(data.CurrentHealth - toLose);
        }
    }

    private void SetHealth(int toSet)
    {
        healthDisplayer.OnSetHealth(toSet);

        data.CurrentHealth = toSet;
    }

    private void Succomb()
    {
        holdingEntity.Deactivate();
        //TODO
    }

    public void SetMaxArmor(int toSet)
    {
        healthDisplayer.OnSetMaxArmor(toSet);

        data.MaxArmor = toSet;
        if (data.CurrentArmor > data.MaxArmor)
        {
            SetArmor(data.MaxArmor);
        }
    }

    public void GainArmor(int toGain)
    {
        healthDisplayer.OnGainArmor(toGain);

        if (data.CurrentArmor + toGain > data.MaxArmor)
        {
            SetArmor(data.MaxArmor);
        }
        else
        {
            SetArmor(data.CurrentArmor + toGain);
        }
    }

    public void LoseArmor(int toLose)
    {
        healthDisplayer.OnLoseArmor(toLose);

        if (data.CurrentArmor > 0)
        {
            if (data.CurrentArmor - toLose <= 0)
            {
                SetArmor(0);
            }
            else
            {
                SetArmor(data.CurrentArmor - toLose);
            }
        }
    }

    private void SetArmor(int toSet)
    {
        healthDisplayer.OnSetArmor(toSet);

        data.CurrentArmor = toSet;
    }
}
