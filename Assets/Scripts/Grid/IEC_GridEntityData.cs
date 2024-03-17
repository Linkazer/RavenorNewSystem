using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_GridEntityData : IEntityData
{
    public bool Walkable { get; }
    public bool BlockVision { get; }
}
