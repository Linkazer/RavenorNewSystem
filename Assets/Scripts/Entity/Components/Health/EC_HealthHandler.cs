using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillDefensiveTrait
{
    Dodge,
    Will,
}

public class EC_HealthHandler : EntityComponent<IEC_HealthHandlerData>
{
    [SerializeField] private ECUI_HealthDisplayer healthDisplayer;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxArmor;
    [SerializeField] private int currentArmor;

    [Header("Animations")]
    [Header("Damage Taken")]
    [SerializeField] private string animationToPlayOnDamageTaken;

    [Header("Death")]
    [SerializeField] private float deathDuration = 1f;
    [SerializeField] private string animationToPlayOnSuccomb;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int MaxArmor => maxArmor;
    public int CurrentArmor => currentArmor;

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

        if (healthDisplayer != null)
        {
            healthDisplayer.OnSetMaxHealth(maxHealth, currentHealth);
        }

        actOnChangeMaxHealth?.Invoke(maxHealth);

        if (currentHealth > maxHealth)
        {
            SetHealth(maxHealth);
        }
    }

    public void GainHealth(int toGain)
    {
        if (healthDisplayer != null)
        {
            healthDisplayer.OnGainHealth(toGain);
        }

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
        if (healthDisplayer != null)
        {
            healthDisplayer.OnLoseHealth(toLose);
        }

        if (currentHealth - toLose <= 0)
        {
            SetHealth(0);
            Succomb();
        }
        else
        {
            TryPlayAnimation(animationToPlayOnDamageTaken);
            SetHealth(currentHealth - toLose);
        }
    }

    private void SetHealth(int toSet)
    {
        currentHealth = toSet;

        if (healthDisplayer != null)
        {
            healthDisplayer.OnSetHealth(currentHealth);
        }

        actOnChangeHealth?.Invoke(currentHealth);
    }

    private void Succomb()
    {
        TryPlayAnimation(animationToPlayOnSuccomb);

        Timer despawnTimer = TimerManager.CreateGameTimer(deathDuration, holdingEntity.Deactivate);
    }

    public void SetMaxArmor(int toSet)
    {
        if (healthDisplayer != null)
        {
            healthDisplayer.OnSetMaxArmor(toSet);
        }

        maxArmor = toSet;

        actOnChangeMaxArmor?.Invoke(maxArmor);

        if (currentArmor > maxArmor)
        {
            SetArmor(maxArmor);
        }
    }

    public void GainArmor(int toGain)
    {
        if (healthDisplayer != null)
        {
            healthDisplayer.OnGainArmor(toGain);
        }

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
        if (healthDisplayer != null)
        {
            healthDisplayer.OnLoseArmor(toLose);
        }

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

        if (healthDisplayer != null)
        {
            healthDisplayer.OnSetArmor(currentArmor);
        }

        actOnChangeArmor?.Invoke(currentArmor);
    }

    private void TryPlayAnimation(string animationName)
    {
        if (HoldingEntity.TryGetEntityComponentOfType(out EC_Renderer rendererComponent))
        {
            if (rendererComponent.AnimHandler != null)
            {
                rendererComponent.AnimHandler.PlayAnimation(animationName);
            }
        }

    }
}
