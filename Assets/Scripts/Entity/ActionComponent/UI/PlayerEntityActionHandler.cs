using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEntityActionHandler : PlayerActionHandler
{
    [SerializeField] private bool forceLockOutsideTurnMode;

    protected abstract EntityActionComponent EntityActionComponentHandled { get; }

    public abstract Type GetEntityActionType { get; }

    /// <summary>
    /// Update the ActionHandler with the Component availability (+ UI).
    /// </summary>
    public abstract void UpdateActionAvailibility();

    /// <summary>
    /// Called by the UI and the Inputs.
    /// </summary>
    public virtual void SelectAction()
    {
        actionHandler.OnSelectAction(this);
        //EntityActionComponentHandled.SelectAction();
    }

    /// <summary>
    /// Called by UI or when an other action is selected.
    /// </summary>
    public virtual void UnselectAction()
    {
        //EntityActionComponentHandled.UnselectAction();
        actionHandler.OnUnselectAction();
    }

    /// <summary>
    /// Use the action at a position.
    /// </summary>
    /// <param name="usePosition">The position where to use the action.</param>
    /// <param name="callback">The callback at the end of the action.</param>
    public virtual void UseAction(Vector2 usePosition, Action callback)
    {
        UndisplayAction();

        if (!isLocked)
        {
            if(forceLockOutsideTurnMode || RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
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
    protected abstract void DisplayAction(Vector3? actionTargetPosition);

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

    public override void SetHandler(PlayerActionManager handler)
    {
        base.SetHandler(handler);

        handler.EntityHandled.TryGetEntityComponentOfType<T>(out entityActionComponentHandled);
    }
}