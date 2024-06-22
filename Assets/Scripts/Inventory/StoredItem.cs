using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the data of a StorableItem present in the Inventory.
/// </summary>
[Serializable]
public class StoredItem
{
    public StorableItem item;
    public int amount;

    public StorableItem_Usable GetItemAsUsable()
    {
        if (item is StorableItem_Usable)
        {
            return item as StorableItem_Usable;
        }

        return null;
    }

    public StoredItem()
    {

    }

    public StoredItem(StorableItem nItem, int nAmount)
    {
        item = nItem;
        amount = nAmount;
    }
}
