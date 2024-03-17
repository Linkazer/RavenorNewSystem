using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_MovementData : IEntityData
{
    public float Speed { get; }
    public float DistanceByTurn { get; }
}
