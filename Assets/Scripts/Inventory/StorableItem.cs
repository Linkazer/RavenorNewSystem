using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the data of an item that can be put in the inventory.
/// </summary>
[CreateAssetMenu(fileName ="Storable Item", menuName ="Inventory/Base Item")]
public class StorableItem : ScriptableObject
{
    [Header("G�n�ral Informations")]
    [SerializeField] protected RVN_Text displayName;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected RVN_Text description;

    [Header("Storage Data")]
    [SerializeField] private int maxStacks = -1;

    public string Name => displayName.GetText();
    public Sprite Icon => icon;
    public virtual string GetDescription()
    {
        return ("<i>" + description.GetText() + "</i>");
    }

    public int MaxStacks => maxStacks;
}
