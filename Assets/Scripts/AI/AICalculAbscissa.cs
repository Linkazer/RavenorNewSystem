using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public abstract class AICalculAbscissa
{
    public abstract float GetAbscissaValue(AIAction plannedAction);
}

public abstract class AICalculAbscissaTargeted : AICalculAbscissa
{
    protected enum TargetType
    {
        Target,
        Caster
    }

    [SerializeField] protected TargetType targetType;

    protected T GetEntityComponentWanted<T>(AIAction plannedAction) where T : EntityComponent
    {
        T componentToReturn = null;

        switch (targetType)
        {
            case TargetType.Target:
                List<T> possibleComponents = plannedAction.skillTarget.GetEntityComponentsOnNode<T>();

                if (possibleComponents.Count > 0)
                {
                    componentToReturn = possibleComponents[0];
                }
                break;
            case TargetType.Caster:
                plannedAction.character.TryGetEntityComponentOfType(out componentToReturn);
                break;
        }

        return componentToReturn;
    }
}

#region Stats
public class AICalculAbcsissa_Strength : AICalculAbscissaTargeted
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_TraitHandler traitHandler = GetEntityComponentWanted<EC_TraitHandler>(plannedAction);

        if (traitHandler != null)
        {
            return traitHandler.Force;
        }
        else
        {
            return 0f;
        }
    }
}

public class AICalculAbcsissa_Esprit : AICalculAbscissaTargeted
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_TraitHandler traitHandler = GetEntityComponentWanted<EC_TraitHandler>(plannedAction);

        if (traitHandler != null)
        {
            return traitHandler.Esprit;
        }
        else
        {
            return 0f;
        }
    }
}

public class AICalculAbcsissa_Presence : AICalculAbscissaTargeted
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_TraitHandler traitHandler = GetEntityComponentWanted<EC_TraitHandler>(plannedAction);

        if (traitHandler != null)
        {
            return traitHandler.Presence;
        }
        else
        {
            return 0f;
        }
    }
}

public class AICalculAbcsissa_Agilite : AICalculAbscissaTargeted
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_TraitHandler traitHandler = GetEntityComponentWanted<EC_TraitHandler>(plannedAction);

        if (traitHandler != null)
        {
            return traitHandler.Agilite;
        }
        else
        {
            return 0f;
        }
    }
}

public class AICalculAbcsissa_Instinct : AICalculAbscissaTargeted
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_TraitHandler traitHandler = GetEntityComponentWanted<EC_TraitHandler>(plannedAction);

        if (traitHandler != null)
        {
            return traitHandler.Instinct;
        }
        else
        {
            return 0f;
        }
    }
}
#endregion

#region Health
public class AICalculAbcsissa_HealthAmount : AICalculAbscissaTargeted
{
    private enum QuantityWanted
    {
        Current,
        Max,
        Percent
    }

    [SerializeField] private QuantityWanted quantityWanted;

    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_HealthHandler healthHandler = GetEntityComponentWanted<EC_HealthHandler>(plannedAction);

        if (healthHandler != null)
        {
            switch (quantityWanted)
            {
                case QuantityWanted.Current:
                    return healthHandler.CurrentHealth;
                case QuantityWanted.Max:
                    return healthHandler.MaxHealth;
                case QuantityWanted.Percent:
                    return (float)healthHandler.CurrentHealth / (float)healthHandler.MaxHealth;
            }
        }
        
        return 0f;
    }
}

public class AICalculAbcsissa_ArmorAmount : AICalculAbscissaTargeted
{
    private enum QuantityWanted
    {
        Current,
        Max,
        Percent
    }

    [SerializeField] private QuantityWanted quantityWanted;

    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_HealthHandler healthHandler = GetEntityComponentWanted<EC_HealthHandler>(plannedAction);

        if (healthHandler != null)
        {
            switch (quantityWanted)
            {
                case QuantityWanted.Current:
                    return healthHandler.CurrentArmor;
                case QuantityWanted.Max:
                    return healthHandler.MaxArmor;
                case QuantityWanted.Percent:
                    return (float)healthHandler.CurrentArmor / (float)healthHandler.MaxArmor;
            }
        }

        return 0f;
    }
}
#endregion

#region Skill Absorber
public class AICalculAbcsissa_Dodge : AICalculAbscissaTargeted
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_SkillAbsorberHandler skillAbsorberHandler = GetEntityComponentWanted<EC_SkillAbsorberHandler>(plannedAction);

        if (skillAbsorberHandler != null)
        {
            return skillAbsorberHandler.Dodge;
        }
        else
        {
            return 0f;
        }
    }
}

public class AICalculAbcsissa_Will : AICalculAbscissaTargeted
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        EC_SkillAbsorberHandler skillAbsorberHandler = GetEntityComponentWanted<EC_SkillAbsorberHandler>(plannedAction);

        if (skillAbsorberHandler != null)
        {
            return skillAbsorberHandler.Will;
        }
        else
        {
            return 0f;
        }
    }
}
#endregion

#region Movement
public abstract class AICalculAbscissa_DistanceCalcul : AICalculAbscissa
{
    protected enum MovementStartPosition
    {
        CurrentPosition,
        CalculatedPosition
    }

    [SerializeField] protected MovementStartPosition startPosition;
}

public class AICalculAbscissa_DistanceWithTarget : AICalculAbscissa_DistanceCalcul
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        switch(startPosition)
        {
            case MovementStartPosition.CurrentPosition:
                plannedAction.character.TryGetEntityComponentOfType(out EC_NodeHandler nodeHandler);

                if(nodeHandler != null)
                {
                    return Pathfinding.Instance.GetDistance(nodeHandler.CurrentNode, plannedAction.skillTarget);
                }
                break;
            case MovementStartPosition.CalculatedPosition:
                return Pathfinding.Instance.GetDistance(plannedAction.movementTarget, plannedAction.skillTarget);
        }

        return 0f;
    }
}

public class AICalculAbscissa_DistanceToTravel : AICalculAbscissa
{
    public override float GetAbscissaValue(AIAction plannedAction)
    {
        plannedAction.character.TryGetEntityComponentOfType(out EC_NodeHandler nodeHandler);

        List<Node> path = Pathfinding.Instance.CalculatePathfinding(nodeHandler.CurrentNode, plannedAction.movementTarget, -1f);
        if(path.Count > 0)
        {
            return Pathfinding.Instance.GetPathLength(nodeHandler.CurrentNode, path);
        }

        return 0f;
    }
}
#endregion

#region Skill
public class AICalculAbscissa_EntitiesInSkillZone : AICalculAbscissa
{
    [SerializeField, Tooltip("L'hostilité se base sur le personnage calculant son coup. (Ex : si il est Hostile, alors mettre HostilityWanted en Hostile cherchera après les personnages Alliés")]
    private CharacterHostility hostilityWanted = CharacterHostility.Neutral;

    public override float GetAbscissaValue(AIAction plannedAction)
    {
        float entitiesInZone = 0f;

        List<Node> zoneNodes = plannedAction.skillToUse.GetDisplayShape(plannedAction.movementTarget, plannedAction.skillTarget);
        foreach (Node zoneNode in zoneNodes)
        {
            List<EC_NodeHandler> entitiesToCheck = zoneNode.EntitiesOnNode;

            foreach (EC_NodeHandler entityNode in entitiesToCheck)
            {
                switch (hostilityWanted)
                {
                    case CharacterHostility.Ally:
                        if (CharacterManager.AreCharacterAlly(plannedAction.character, entityNode.HoldingEntity) == 1)
                        {
                            entitiesInZone++;
                        }
                        break;
                    case CharacterHostility.Hostile:
                        if (CharacterManager.AreCharacterAlly(plannedAction.character, entityNode.HoldingEntity) == -1)
                        {
                            entitiesInZone++;
                        }
                        break;
                    case CharacterHostility.Neutral:
                        entitiesInZone++;
                        break;
                }
            }
        }

        return entitiesInZone;
    }
}
#endregion
