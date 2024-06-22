using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle the button of an Item in the Inventory UI.
/// </summary>
public class StoredItemButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemAmount;

    private StoredItem linkedItem;

    /// <summary>
    /// Set the item held by the button.
    /// </summary>
    /// <param name="nLinkedItem">The item to hold.</param>
    public void SetItem(StoredItem nLinkedItem)
    {
        if (nLinkedItem == null)
        {
            linkedItem = null;
            gameObject.SetActive(false);
        }
        else
        {
            linkedItem = nLinkedItem;

            itemIcon.sprite = linkedItem.item.Icon;
            itemAmount.text = linkedItem.amount.ToString();

            gameObject.SetActive(true);

            UpdateUsability();
        }
    }

    /// <summary>
    /// Update the usability of the item.
    /// </summary>
    public void UpdateUsability()
    {
        canvasGroup.interactable = linkedItem.amount > 0 && linkedItem.item is StorableItem_Usable;
    }
}
