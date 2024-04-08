using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_InventoryHandler : EntityActionComponent<IEC_InventoryHandlerData>
{
    public override void SetComponentData(IEC_InventoryHandlerData componentData)
    {
        foreach (StoredItem itm in componentData.StoredItems)
        {
            InventoryManager.Instance.AddItem(itm.item, itm.amount);
        }
    }

    protected override void InitializeComponent()
    {
        
    }

    public override void Activate()
    {

    }

    public override void Deactivate()
    {

    }

    public override void StartRound()
    {

    }

    public override void EndRound()
    {
        
    }

    public override bool IsActionAvailable()
    {
        return true;
    }

    protected override void OnUseAction(Vector3 actionTargetPosition)
    {
        
    }

    public override void CancelAction()
    {
        
    }

    protected override void OnEndAction()
    {
        
    }
}
