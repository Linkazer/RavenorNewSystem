using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_StatusHandlerData : IEntityData
{
    public StatusData[] Passives { get; }
}
