using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the data for a StorableItem that can be used by a SkillHandler.
/// </summary>
[CreateAssetMenu(fileName = "Usable Storable Item", menuName = "Inventory/Usable Item")]
public class StorableItem_Usable : StorableItem
{
    [SerializeField] private SKL_SkillScriptable objectAction;

    [SerializeField] private int quantityLostOnUse = 1;

    public int QuantityLostOnUse => quantityLostOnUse;
    public SKL_SkillScriptable ObjectAction => objectAction;

    public override string GetDescription()
    {
        return objectAction.Description + "\n\n" + base.GetDescription();
    }
}
