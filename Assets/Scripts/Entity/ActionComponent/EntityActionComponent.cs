using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityActionComponent : EntityComponent
{
    protected Action endActionCallback;

    /// <summary>
    /// Utilisé pour savoir si l'action peut être sélectionnée.
    /// </summary>
    /// <returns></returns>
    public abstract bool IsActionAvailable();

    public abstract void SelectAction();

    public abstract void UnselectAction();

    public abstract void DisplayAction(Vector3 actionTargetPosition);

    public abstract void UndisplayAction();

    /// <summary>
    /// Utilisé pour savoir si l'action peut être utilisée.
    /// </summary>
    /// <returns></returns>
    public abstract bool IsActionUsable(Vector3 positionToCheck);

    public void DoAction(Vector3 actionTargetPosition, Action endCallback)
    {
        endActionCallback = endCallback;

        OnDoAction(actionTargetPosition);
    }

    protected abstract void OnDoAction(Vector3 actionTargetPosition);

    public abstract void CancelAction();

    public void EndAction()
    {
        OnEndAction();

        endActionCallback?.Invoke();
        endActionCallback = null;
    }

    protected abstract void OnEndAction();
}

public abstract class EntityActionComponent<T> : EntityActionComponent where T : IEntityData
{
    /// <summary>
    /// Set the Data of the EntityComponent.
    /// </summary>
    /// <param name="componentData">The data to set from.</param>
    public abstract void SetComponentData(T componentData);

    public override void SetComponent(Entity componentEntityHandler)
    {
        holdingEntity = componentEntityHandler;

        if (componentEntityHandler.GetComponentData() is T)
        {
            SetComponentData((T)componentEntityHandler.GetComponentData());
        }

        InitializeComponent();
    }
}
