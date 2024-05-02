using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private List<StoredItem> storedItemsInInventory = new List<StoredItem>();

    public Action actOnUpdateInventory;

    public List<StoredItem> StoredItemsInInventory => storedItemsInInventory;

    public void AddItem(StorableItem item, int quantityToAdd)
    {
        int indexInInventory = ContainsItem(item);
        if (indexInInventory < 0)
        {
            storedItemsInInventory.Add(new StoredItem(item, quantityToAdd));
            indexInInventory = storedItemsInInventory.Count - 1;
        }

        storedItemsInInventory[indexInInventory].amount += quantityToAdd;

        if (item.MaxStacks > 0 && storedItemsInInventory[indexInInventory].amount > item.MaxStacks)
        {
            storedItemsInInventory[indexInInventory].amount = item.MaxStacks;
        }

        actOnUpdateInventory?.Invoke();
    }

    public void RemoveItem(StorableItem item, int quantityToRemove = 1)
    {
        StoredItem itemToRemove = null;

        foreach (StoredItem itm in storedItemsInInventory)
        {
            if (itm.item == item)
            {
                itm.amount -= quantityToRemove;

                if (itm.amount <= 0)
                {
                    itemToRemove = itm;
                }
            }
        }

        if(itemToRemove != null)
        {
            storedItemsInInventory.Remove(itemToRemove);
        }

        actOnUpdateInventory?.Invoke();
    }

    public int ContainsItem(StorableItem item)
    {
        for(int i = 0; i < storedItemsInInventory.Count; i++)
        {
            if(item == storedItemsInInventory[i].item)
            {
                return i;
            }
        }

        return -1;
    }
}