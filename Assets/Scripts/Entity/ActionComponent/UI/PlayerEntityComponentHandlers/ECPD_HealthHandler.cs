using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ECPD_HealthHandler : PlayerEntityComponentDisplay<EC_HealthHandler>
{
    private EC_HealthHandler healthHandler => componentHandled;

    [SerializeField] private Image portrait;

    [SerializeField] private Image healthImageToFill;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    protected override void SetComponent(EC_HealthHandler entityComponent)
    {
        portrait.sprite = characterEntity.CharacterData.Portrait;

        SetMaxHealth(healthHandler.MaxHealth);
        SetCurrentHealth(healthHandler.CurrentHealth);

        healthHandler.actOnChangeMaxHealth += SetMaxHealth;
        healthHandler.actOnChangeHealth += SetCurrentHealth;
    }

    protected override void UnsetComponent(EC_HealthHandler entityComponent)
    {
        healthHandler.actOnChangeMaxHealth -= SetMaxHealth;
        healthHandler.actOnChangeHealth -= SetCurrentHealth;
    }

    private void SetMaxHealth(int maxHealth)
    {
        maxHealthText.text = maxHealth.ToString();
        UpdateFillImage();
    }

    private void SetCurrentHealth(int currentHealth)
    {
        currentHealthText.text = currentHealth.ToString();
        UpdateFillImage();
    }

    private void UpdateFillImage()
    {
        healthImageToFill.fillAmount = 1f - (float)healthHandler.CurrentHealth / (float)healthHandler.MaxHealth;
    }
}
