using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AICalcul
{
    public float calculImportance;
    [SerializeField, SerializeReference, ReferenceEditor(typeof(AICalculAbscissa))] protected AICalculAbscissa abcissaCalcul;

    public abstract float Calculate(AIAction plannedAction);
}

[Serializable]
public class AICalcul_Conditional : AICalcul
{
    public enum ConditionalWanted
    {
        More,
        MoreOrEqual,
        Equal,
        LessOrEqual,
        Less,
    }

    [SerializeField] private ConditionalWanted condition = ConditionalWanted.Equal;
    [SerializeField] private float baseValue;

    public override float Calculate(AIAction plannedAction)
    {
        float toReturn = 0;

        float abscissa = abcissaCalcul.GetAbcissaValue(plannedAction);

        switch (condition)
        {
            case ConditionalWanted.More:
                if (abscissa > baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.MoreOrEqual:
                if (abscissa >= baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.Equal:
                if (abscissa == baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.LessOrEqual:
                if (abscissa <= baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.Less:
                if (abscissa < baseValue)
                {
                    toReturn = 1;
                }
                break;
        }

        return toReturn;
    }
}

public class AICalcul_Affine : AICalcul
{
    [SerializeField] private float abscissaCoeficient;
    [SerializeField] private float constantToAdd;
    [SerializeField, Tooltip("Valeur attendue pour que le score face 1.")] private float maxValue;

    public override float Calculate(AIAction plannedAction)
    {
        float abscissa = abcissaCalcul.GetAbcissaValue(plannedAction);

        return (constantToAdd + abscissa * abscissaCoeficient) / maxValue;
    }
}