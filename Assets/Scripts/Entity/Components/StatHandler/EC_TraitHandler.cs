using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityTraits
{
    Force,
    Esprit,
    Presence,
    Agilite,
    Instinct
}

public class EC_TraitHandler : EntityComponent<IEC_TraitHandlerData>
{
    [SerializeField] private int force;
    [SerializeField] private int esprit;
    [SerializeField] private int presence;
    [SerializeField] private int agilite;
    [SerializeField] private int instinct;

    [SerializeField] private List<ChallengeExpertiseLevel> expertises;

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

    public List<ChallengeExpertiseLevel> Expertises => expertises;


    public override void SetComponentData(IEC_TraitHandlerData componentData)
    {
        force = componentData.Force;
        agilite = componentData.Agilite;
        instinct = componentData.Instinct;
        esprit = componentData.Esprit;
        presence = componentData.Presence;

        expertises = componentData.Expertises;
    }

    protected override void InitializeComponent()
    {
        
    }

    private bool HasExpertise(ChallengeExpertise expertiseWanted)
    {
        foreach(ChallengeExpertiseLevel expertiseLevel in  expertises)
        {
            if(expertiseLevel.expertise == expertiseWanted)
            {
                return true;
            }
        }

        return false;
    }

    public int GetExpertiseLevel(ChallengeExpertise expertiseWanted)
    {
        foreach (ChallengeExpertiseLevel expertiseLevel in expertises)
        {
            if (expertiseLevel.expertise == expertiseWanted)
            {
                return expertiseLevel.level;
            }
        }

        return 0;
    }

    public int GetExpertiseTraitValue(ChallengeExpertise expertiseWanted)
    {
        return GetTraitValue(expertiseWanted.Trait);

        //Si on veut donner le bonus de trait uniquement si le personnage possède l'expertise :
        /*if(HasExpertise(expertiseWanted))
        {
            return GetTraitValue(expertiseWanted.Trait);
        }

        return 0;*/
    }

    public int GetTraitValue(EntityTraits wantedTrait)
    {
        switch(wantedTrait)
        {
            case EntityTraits.Force:
                return Force;
            case EntityTraits.Agilite:
                return Agilite;
            case EntityTraits.Esprit:
                return Esprit;
            case EntityTraits.Presence:
                return Presence;
            case EntityTraits.Instinct:
                return Instinct;
        }

        return 0;
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
