using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEntityActionInput
{
    protected PlayerEntityActionHandler actionHandler;

    protected abstract EntityActionComponent EntityActionComponentHandled { get; }

    public abstract Type GetEntityActionType { get; }

    public void Enable()
    {

    }

    public void Disable()
    {

    }

    public virtual void SetHandler(PlayerEntityActionHandler handler)
    {
        actionHandler = handler;
    }

    public bool IsActionAvailable()
    {
        return EntityActionComponentHandled.IsActionAvailable();
    }

    public virtual void SelectAction()
    {
        EntityActionComponentHandled.SelectAction();
    }

    public virtual void UnselectAction()
    {
        EntityActionComponentHandled.UnselectAction();
    }

    public void DisplayAction(Vector3 actionTargetPosition)
    {
        EntityActionComponentHandled.DisplayAction(actionTargetPosition);
    }

    public void UndisplayAction()
    {
        EntityActionComponentHandled.UndisplayAction();
    }

    public bool IsActionUsable(Vector3 positionToCheck)
    {
        return EntityActionComponentHandled.IsActionUsable(positionToCheck);
    }

    public void DoAction(Vector3 actionTargetPosition)
    {
        EntityActionComponentHandled.DoAction(actionTargetPosition, EndAction);
    }

    public void CancelAction()
    {
        EntityActionComponentHandled.CancelAction();
    }

    protected void EndAction()
    {

    }
}

public abstract class PlayerEntityActionInput<T> : PlayerEntityActionInput where T : EntityActionComponent
{
    protected T entityActionComponentHandled;

    protected override EntityActionComponent EntityActionComponentHandled => entityActionComponentHandled;

    public override Type GetEntityActionType => typeof(T);

    public override void SetHandler(PlayerEntityActionHandler handler)
    {
        base.SetHandler(handler);

        handler.ActionDoerHandled.TryGetActionComponentOfType<T>(out entityActionComponentHandled);
    }
}