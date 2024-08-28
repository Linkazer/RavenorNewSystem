using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillRessource
{
    [SerializeField] protected SkillRessourceType ressourceType;
    [SerializeField] protected int startAmount;
    [SerializeField] protected Vector2Int limits;
    [SerializeField] protected int currentAmount;

    public Action<int> actOnUpdateRessource;

    protected EC_SkillHandler skillHandler;

    public int CurrentAmount => currentAmount;

    public SkillRessourceType RessourceType => ressourceType;

    public abstract SkillRessource GetAsNew();

    public virtual void Initialize(EC_SkillHandler nSkillHandler)
    {
        skillHandler = nSkillHandler;
        currentAmount = startAmount;
    }

    public virtual void Ativate()
    {
        
    }

    public virtual void Deactivate()
    {
        skillHandler = null;
    }

    public abstract bool HasEnoughRessource(int amountNeeded);

    public abstract void OnUseSkillWithRessource(int ressourceAmount);

    public void AddRessource(int amountToAdd)
    {
        currentAmount += amountToAdd;

        if(currentAmount > limits.y)
        {
            currentAmount = limits.y;
        }

        actOnUpdateRessource?.Invoke(currentAmount);
    }

    public void RemoveRessource(int amountToRemove)
    {
        currentAmount -= amountToRemove;

        if (currentAmount < limits.x)
        {
            currentAmount = limits.x;
        }

        actOnUpdateRessource?.Invoke(currentAmount);

        Debug.Log(currentAmount);
    }
}