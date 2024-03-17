using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEntityActionHandler : MonoBehaviour
{
    protected PlayerEntityActionManager actionHandler;

    protected bool isLocked;

    protected abstract EntityActionComponent EntityActionComponentHandled { get; }

    public abstract Type GetEntityActionType { get; }

    public virtual void SetHandler(PlayerEntityActionManager handler)
    {
        actionHandler = handler;
    }

    /// <summary>
    /// Active l'ActionHandler (+ UI).
    /// </summary>
    public abstract void Enable();

    /// <summary>
    /// Désactive l'ActionHandler (+ UI).
    /// </summary>
    public abstract void Disable();

    /// <summary>
    /// Bloque l'ActionHandler (+ UI).
    /// </summary>
    /// <param name="doesLock">Should be locked ?</param>
    public void Lock(bool doesLock)
    {
        if(doesLock != isLocked)
        {
            isLocked = doesLock;
            UpdateActionAvailibility();
        }
    }

    /// <summary>
    /// Update the ActionHandler with the Component availability (+ UI).
    /// </summary>
    public abstract void UpdateActionAvailibility();

    /// <summary>
    /// Called by the UI and the Inputs.
    /// </summary>
    public virtual void SelectAction()
    {
        actionHandler.SelectAction(this);
        EntityActionComponentHandled.SelectAction();
    }

    /// <summary>
    /// Called by UI or when an other action is selected.
    /// </summary>
    public virtual void UnselectAction()
    {
        EntityActionComponentHandled.UnselectAction();
        actionHandler.UnselectAction();
    }

    public virtual void UseAction(Vector2 usePosition)
    {
        EntityActionComponentHandled.UseAction(usePosition, EndAction);
        actionHandler.OnUseAction(EntityActionComponentHandled);
    }

    protected virtual bool IsActionAvailable()
    {
        return EntityActionComponentHandled.IsActionAvailable();
    }

    protected virtual void DisplayAction(Vector3 actionTargetPosition)
    {
        EntityActionComponentHandled.DisplayAction(actionTargetPosition);
    }

    protected virtual void UndisplayAction()
    {
        EntityActionComponentHandled.UndisplayAction();
    }

    protected virtual bool IsActionUsable(Vector3 positionToCheck)
    {
        return EntityActionComponentHandled.IsActionUsable(positionToCheck);
    }

    protected virtual void CancelAction()
    {
        EntityActionComponentHandled.CancelAction();
    }

    protected virtual void EndAction()
    {
        actionHandler.OnEndAction(EntityActionComponentHandled);
    }
}

public abstract class PlayerEntityActionHandler<T> : PlayerEntityActionHandler where T : EntityActionComponent
{
    protected T entityActionComponentHandled;

    protected override EntityActionComponent EntityActionComponentHandled => entityActionComponentHandled;

    public override Type GetEntityActionType => typeof(T);

    public override void SetHandler(PlayerEntityActionManager handler)
    {
        base.SetHandler(handler);

        handler.EntityHandled.TryGetComponentOfType<T>(out entityActionComponentHandled);
    }
}