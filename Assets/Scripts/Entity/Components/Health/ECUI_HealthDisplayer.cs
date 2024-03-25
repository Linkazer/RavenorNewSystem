using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECUI_HealthDisplayer : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image[] armorDisplayed; //TODO : Faire des scripts pour l'affichage des Armure avec animation

    private float maxHealthBarFill;
    private float currentHealthBarFill;

    public void OnSetMaxHealth(int amountToSet, int currentHealth)
    {
        maxHealthBarFill = amountToSet;
        currentHealthBarFill = currentHealth;

        healthBar.fillAmount = currentHealthBarFill / maxHealthBarFill;
    }

    public void OnSetMaxArmor(int amountToSet)
    {

    }

    public void OnSetHealth(int amountToSet)
    {
        currentHealthBarFill = amountToSet;

        healthBar.fillAmount = currentHealthBarFill / maxHealthBarFill;
    }

    public void OnSetArmor(int amountToSet)
    {

    }

    //Feedback Lose Health
    public void OnGainHealth(int amountToGain)
    {

    }

    //Feedback Gain Health
    public void OnLoseHealth(int amountToLose)
    {

    }

    //Feedback Lose Armor
    public void OnGainArmor(int amountToGain)
    {

    }

    //Feedback Gain Armor
    public void OnLoseArmor(int amountToLose)
    {

    }
}