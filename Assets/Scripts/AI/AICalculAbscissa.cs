using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AICalculAbscissa
{
    public abstract float GetAbcissaValue(AIAction plannedAction);
}

[Serializable]
public class AI_CA_DistanceFromTarget_CalculatedPosition : AICalculAbscissa
{
    public override float GetAbcissaValue(AIAction plannedAction)
    {
        return Pathfinding.Instance.GetDistance(plannedAction.movementTarget, plannedAction.skillTarget);
    }
}