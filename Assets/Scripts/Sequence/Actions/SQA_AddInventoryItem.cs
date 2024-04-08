using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_AddInventoryItem : SequenceAction
{
    [SerializeField] private StoredItem[] itemsToAdd;

    protected override void OnStartAction()
    {
        foreach (StoredItem itemToAdd in itemsToAdd)
        {
            InventoryManager.Instance.AddItem(itemToAdd.item, itemToAdd.amount);
        }
        EndAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        foreach (StoredItem itemToAdd in itemsToAdd)
        {
            InventoryManager.Instance.AddItem(itemToAdd.item, itemToAdd.amount);
        }
    }
}
