using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_AddInventoryItem : SequenceAction
{
    [SerializeField] private StoredItem[] itemsToAdd;

    protected override void OnStartAction(SequenceContext context)
    {
        foreach (StoredItem itemToAdd in itemsToAdd)
        {
            InventoryManager.Instance.AddItem(itemToAdd.item, itemToAdd.amount);
        }
        EndAction(context);
    }

    protected override void OnEndAction(SequenceContext context)
    {
        
    }

    protected override void OnSkipAction(SequenceContext context)
    {
        foreach (StoredItem itemToAdd in itemsToAdd)
        {
            InventoryManager.Instance.AddItem(itemToAdd.item, itemToAdd.amount);
        }
    }
}
