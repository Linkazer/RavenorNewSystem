using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoredItemButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemAmount;

    private StoredItem linkedItem;

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

    public void UpdateUsability()
    {
        canvasGroup.interactable = linkedItem.amount > 0 && linkedItem.item is StorableItem_Usable;
    }
}
