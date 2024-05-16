using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_SkillAbsorberHandler : EntityComponent<IEC_SkillAbsorberHandlerData>
{
    private const int BaseDodge = 4;
    private const int BaseWill = 4;

    [SerializeField] private int defensiveAdvantage;
    [SerializeField] private int defensiveDisavantage;

    public int dodgeBonus;
    public int willBonus;

    public int DefensiveAdvantage => defensiveAdvantage;

    public int DefensiveDisadvantage => defensiveDisavantage;

    private EC_TraitHandler traits;

    public int Dodge
    {
        get
        {
            if(traits == null)
            {
                return BaseDodge + dodgeBonus;
            }
            else
            {
                return BaseDodge + dodgeBonus + traits.Agilite;
            }
        }
    }

    public int Will
    {
        get
        {
            if (traits == null)
            {
                return BaseWill + willBonus;
            }
            else
            {
                return BaseWill + willBonus + traits.Esprit;
            }
        }
    }

    public override void SetComponentData(IEC_SkillAbsorberHandlerData componentData)
    {
        dodgeBonus = componentData.DodgeBonus;
        willBonus = componentData.WillBonus;

        defensiveAdvantage = componentData.DefensiveAdvantage;
        defensiveDisavantage = componentData.DefensiveDisavantage;
    }

    protected override void InitializeComponent()
    {
        HoldingEntity.TryGetEntityComponentOfType<EC_TraitHandler>(out traits);
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
