using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEC_InventoryHandlerData : IEntityData
{
    public StoredItem[] StoredItems { get; }
}
