using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_TraitHandler : EntityComponent<IEC_TraitHandlerData>
{
    [SerializeField] private int force;
    [SerializeField] private int esprit;
    [SerializeField] private int presence;
    [SerializeField] private int agilite;
    [SerializeField] private int instinct;

    private int forceBonus;
    private int espritBonus;
    private int presenceBonus;
    private int agiliteBonus;
    private int instinctBonus;

    public int Force => force + forceBonus;
    public int Esprit => esprit + espritBonus;
    public int Presence => presence + presenceBonus;
    public int Agilite => agilite + agiliteBonus;
    public int Instinct => instinct + instinctBonus;


    public override void SetComponentData(IEC_TraitHandlerData componentData)
    {
        force = componentData.Force;
        agilite = componentData.Agilite;
        instinct = componentData.Instinct;
        esprit = componentData.Esprit;
        presence = componentData.Presence;
    }

    protected override void InitializeComponent()
    {
        
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
}
