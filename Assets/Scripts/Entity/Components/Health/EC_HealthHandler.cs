using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_HealthHandler : EntityComponent<IEC_HealthHandlerData>
{
    [SerializeField] private ECUI_HealthDisplayer healthDisplayer;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxArmor;
    [SerializeField] private int currentArmor;
    [SerializeField] private int dodge;
    [SerializeField] private int defensiveAdvantage;
    [SerializeField] private int defensiveDisavantage;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int MaxArmor => maxArmor;
    public int CurrentArmor => currentArmor;
    public int Dodge => dodge > 0 ? dodge : 0;
    public int DefensiveAdvantage => defensiveAdvantage;
    public int DefensiveDisavantage => defensiveDisavantage;

    public Action<int> actOnChangeMaxHealth;
    public Action<int> actOnChangeHealth;
    public Action<int> actOnChangeMaxArmor;
    public Action<int> actOnChangeArmor;

    public override void SetComponentData(IEC_HealthHandlerData componentData)
    {
        maxHealth = componentData.MaxHealth;
        currentHealth = componentData.CurrentHealth;
        maxArmor = componentData.MaxArmor;
        currentArmor = componentData.CurrentArmor;
        dodge = componentData.Dodge;
        defensiveAdvantage = componentData.DefensiveAdvantage;
        defensiveDisavantage = componentData.DefensiveDisavantage;

        if(healthDisplayer != null)
        {
            healthDisplayer.transform.localPosition = new Vector3(0, componentData.UiHeight, 0);
        }
    }

    protected override void InitializeComponent()
    {
        SetMaxHealth(maxHealth);
        SetMaxArmor(maxArmor);
        SetHealth(currentHealth);
        SetArmor(currentArmor);
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
        maxHealth = toSet;

        healthDisplayer.OnSetMaxHealth(maxHealth, currentHealth);

        actOnChangeMaxHealth?.Invoke(maxHealth);

        if (currentHealth > maxHealth)
        {
            SetHealth(maxHealth);
        }
    }

    public void GainHealth(int toGain)
    {
        healthDisplayer.OnGainHealth(toGain);

        if(currentHealth + toGain > maxHealth)
        {
            SetHealth(maxHealth);
        }
        else
        {
            SetHealth(currentHealth + toGain);
        }
    }

    public void LoseHealth(int toLose)
    {
        healthDisplayer.OnLoseHealth(toLose);

        if (currentHealth - toLose <= 0)
        {
            SetHealth(0);
            Succomb();
        }
        else
        {
            SetHealth(currentHealth - toLose);
        }
    }

    private void SetHealth(int toSet)
    {
        currentHealth = toSet;

        healthDisplayer.OnSetHealth(currentHealth);

        actOnChangeHealth?.Invoke(currentHealth);
    }

    private void Succomb()
    {
        holdingEntity.Deactivate();
        //TODO : Animations et tout
    }

    public void SetMaxArmor(int toSet)
    {
        healthDisplayer.OnSetMaxArmor(toSet);

        maxArmor = toSet;

        actOnChangeMaxArmor?.Invoke(maxArmor);

        if (currentArmor > maxArmor)
        {
            SetArmor(maxArmor);
        }
    }

    public void GainArmor(int toGain)
    {
        healthDisplayer.OnGainArmor(toGain);

        if (currentArmor + toGain > maxArmor)
        {
            SetArmor(maxArmor);
        }
        else
        {
            SetArmor(currentArmor + toGain);
        }
    }

    public void LoseArmor(int toLose)
    {
        healthDisplayer.OnLoseArmor(toLose);

        if (currentArmor > 0)
        {
            if (currentArmor - toLose <= 0)
            {
                SetArmor(0);
            }
            else
            {
                SetArmor(currentArmor - toLose);
            }
        }
    }

    private void SetArmor(int toSet)
    {
        currentArmor = toSet;

        healthDisplayer.OnSetArmor(currentArmor);

        actOnChangeArmor?.Invoke(currentArmor);
    }

    public void AddDodgeBonus(int amount)
    {
        dodge += amount;
    }

    public void RemoveDodgeBonus(int amount)
    {
        dodge -= amount;
    }
}
