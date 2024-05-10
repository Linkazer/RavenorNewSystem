using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker_PossessInventoryItem : Checker
{
    [SerializeField] private StorableItem itemToCheck;
    [SerializeField] private int minimumAmount = 1;

    public override bool IsCheckValid()
    {
        return InventoryManager.Instance.GetItemAmount(itemToCheck) >= minimumAmount;
    }
}
