using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityComponent : MonoBehaviour, IRoundHandler
{
    protected Entity holdingEntity;

    public Entity HoldingEntity => holdingEntity;

    /// <summary>
    /// Set the EntityComponent, and link it with its Entity.
    /// </summary>
    /// <param name="componentEntityHolder">The Entity holding the Component.</param>
    public virtual void SetComponent(Entity componentEntityHolder)
    {
        holdingEntity = componentEntityHolder;

        InitializeComponent();
    }

    /// <summary>
    /// Initialize the Component. Called when all the data are sets.
    /// </summary>
    protected abstract void InitializeComponent();

    private void OnEnable()
    {
        Activate();
    }

    private void OnDisable()
    {
        Deactivate();
    }

    /// <summary>
    /// Activate the Component.
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Deactivate the Component.
    /// </summary>
    public abstract void Deactivate();

    /// <summary>
    /// Called when the Entity start a Round.
    /// </summary>
    public abstract void StartRound();

    /// <summary>
    /// Called when the Entity end a Round.
    /// </summary>
    public abstract void EndRound();
}

public abstract class EntityComponent<T> : EntityComponent where T : IEntityData
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