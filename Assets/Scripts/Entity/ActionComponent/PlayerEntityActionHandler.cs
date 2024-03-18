using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEntityActionHandler : MonoBehaviour
{
    protected PlayerEntityActionManager actionHandler;

    [SerializeField] protected bool isLocked;

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
    /// D�sactive l'ActionHandler (+ UI).
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

    /// <summary>
    /// Use the action at a position.
    /// </summary>
    /// <param name="usePosition">The position where to use the action.</param>
    /// <param name="callback">The callback at the end of the action.</param>
    public virtual void UseAction(Vector2 usePosition, Action callback)
    {
        if (!isLocked)
        {
            if(RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
            {
                actionHandler.AddLock(this);
            }

            EntityActionComponentHandled.UseAction(usePosition, callback);
            actionHandler.OnUseAction(EntityActionComponentHandled);
        }
    }

    /// <summary>
    /// Use the action at a position.
    /// </summary>
    /// <param name="usePosition">The position where to use the action.</param>
    public virtual void UseAction(Vector2 usePosition)
    {
        UseAction(usePosition, EndAction);
    }

    /// <summary>
    /// Check if the action is available.
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsActionAvailable()
    {
        return EntityActionComponentHandled.IsActionAvailable();
    }

    /// <summary>
    /// Display the action.
    /// </summary>
    /// <param name="actionTargetPosition"></param>
    protected abstract void DisplayAction(Vector3 actionTargetPosition);

    /// <summary>
    /// Undisplay the action.
    /// </summary>
    protected abstract void UndisplayAction();

    /// <summary>
    /// Called at the end of the action.
    /// </summary>
    protected virtual void EndAction()
    {
        actionHandler.RemoveLock(this);
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