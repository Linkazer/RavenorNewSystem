using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillHolder
{
    [SerializeField] private SKL_SkillScriptable scriptable;

    private RoundTimer cooldownTimer;

    private int utilisationByTurnLeft = 1;
    private int utilisationByLevelLeft = -1;

    public Action<int> OnUpdateCooldown;

    public int CurrentCooldown => cooldownTimer != null ? Mathf.CeilToInt(cooldownTimer.roundLeft) : 0;

    public SKL_SkillScriptable Scriptable => scriptable;

    public SkillHolder(SKL_SkillScriptable skillScriptable)
    {
        scriptable = skillScriptable;

        cooldownTimer = null;
        if (scriptable.MaxUtilisationsByLevel > 0)
        {
            utilisationByLevelLeft = scriptable.MaxUtilisationsByLevel;
        }
        else
        {
            utilisationByLevelLeft = -1;
        }

        if (scriptable.MaxUtilisationsByTurn > 1)
        {
            utilisationByTurnLeft = scriptable.MaxUtilisationsByTurn;
        }
        else
        {
            utilisationByTurnLeft = 1;
        }
    }

    public bool IsUsable()
    {
        return CurrentCooldown <= 0 && utilisationByLevelLeft != 0;
    }

    public void UseSkill()
    {
        utilisationByLevelLeft--;
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round) //Voir si on garde ça et si on met pas le reset à chaque tour du Personnage en mode temps réel
        {
            utilisationByTurnLeft--;
        }

        StartCooldown();
    }

    public void StartCooldown()
    {
        SetCooldown(scriptable.Cooldown);
    }

    public void SetCooldown(int valueToSet)
    {
        cooldownTimer = RoundManager.Instance.CreateRoundTimer(valueToSet, UpdateCooldown, EndCooldownTimer);
        OnUpdateCooldown?.Invoke(CurrentCooldown);
    }

    public void ProgressCooldown()
    {
        if (CurrentCooldown > 0)
        {
            cooldownTimer.ProgressRound(1);

            UpdateCooldown();
        }
    }

    public void UpdateCooldown()
    {
        if (cooldownTimer != null && cooldownTimer.roundLeft > 0)
        {
            if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
            {
                cooldownTimer.ProgressRound(1);
            }

            OnUpdateCooldown?.Invoke(CurrentCooldown);
        }
    }

    private void EndCooldownTimer()
    {
        cooldownTimer = null;
        OnUpdateCooldown?.Invoke(CurrentCooldown);
    }
}
