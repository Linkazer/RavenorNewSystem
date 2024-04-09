using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ECAH_Inventory : PlayerEntityActionHandler<EC_InventoryHandler>
{
    [SerializeField] private ECAH_SkillHandler skillActionHandler;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private StoredItemButton[] storedItemButtons;

    public override void UpdateActionAvailibility()
    {
        UpdateDisplayedItems();
    }

    protected override void DisplayAction(Vector3 actionTargetPosition)
    {
        
    }

    protected override void UndisplayAction()
    {
        
    }

    public override void Enable()
    {
        canvasGroup.alpha = 1f;
        InventoryManager.Instance.actOnUpdateInventory += UpdateDisplayedItems;
        UpdateDisplayedItems();
    }

    public override void Disable()
    {
        canvasGroup.alpha = 0f;
        InventoryManager.Instance.actOnUpdateInventory -= UpdateDisplayedItems;
    }

    public override void UnselectAction()
    {
        base.UnselectAction();
    }

    private void UpdateDisplayedItems()
    {
        for (int i = 0; i < storedItemButtons.Length; i++)
        {
            if(i < InventoryManager.Instance.StoredItemsInInventory.Count)
            {
                storedItemButtons[i].SetItem(InventoryManager.Instance.StoredItemsInInventory[i]);
            }
            else
            {
                storedItemButtons[i].SetItem(null);
            }
        }
    }

    public void UE_SelectItem(int itemIndex)
    {
        if (itemIndex < InventoryManager.Instance.StoredItemsInInventory.Count)
        {
            skillActionHandler.SelectSkill((InventoryManager.Instance.StoredItemsInInventory[itemIndex].item as StorableItem_Usable).ObjectAction);
        }
    }
}
