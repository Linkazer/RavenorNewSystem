using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IRoundHandler
{
    [SerializeField] protected EntityComponent[] components;

    private Dictionary<Type, EntityComponent> componentByType = new Dictionary<Type, EntityComponent>();

    public Dictionary<Type, EntityComponent> ComponentsByType => componentByType;

    /// <summary>
    /// Get the IEntityData from the data holder. The data holder depends on the Entity type used 
    /// (Ex : The CharacterEntity has a CharacterScriptable that contains multiple IEntityData)
    /// </summary>
    /// <returns></returns>
    public virtual IEntityData GetComponentData()
    {
        return null;
    }

    /// <summary>
    /// Try to get an EntityComponent by its type.
    /// </summary>
    /// <typeparam name="T">The type of the EntityComponent wanted.</typeparam>
    /// <param name="foundComponent">The found EntityComponent.</param>
    /// <returns>TRUE if an EntityComponent is found. Otherwise, FALSE.</returns>
    public bool TryGetComponentOfType<T>(out T foundComponent) where T : EntityComponent
    {
        foundComponent = null;

        if(componentByType.ContainsKey(typeof(T)))
        {
            foundComponent = componentByType[typeof(T)] as T;
        }

        return foundComponent != null;
    }

    private void Start()
    {
        SetComponents();
        Activate();
    }

    /// <summary>
    /// Set every EntityComponent holded by the Entity.
    /// </summary>
    protected virtual void SetComponents()
    {
        foreach (EntityComponent component in components)
        {
            if (!componentByType.ContainsKey(component.GetType()))
            {
                componentByType.Add(component.GetType(), component);
            }
            else
            {
                Debug.LogError($"!!! Component of type {component.GetType()} exist multiple times in {this} !!!");
            }

            component.SetComponent(this);
        }
    }

    /// <summary>
    /// Activate every EntityComponent.
    /// </summary>
    public void Activate()
    {
        foreach (EntityComponent component in components)
        {
            component.Activate();
        }

        RoundManager.Instance.AddHandler(this);
    }

    /// <summary>
    /// Deactivate every EntityComponent.
    /// </summary>
    public void Deactivate()
    {
        RoundManager.Instance.RemoveHandler(this);

        foreach (EntityComponent component in components)
        {
            component.Deactivate();
        }
    }

    /// <summary>
    /// Trigger a StartRound for every EntityComponent.
    /// </summary>
    public virtual void StartRound()
    {
        foreach (EntityComponent component in components)
        {
            component.StartRound();
        }
    }

    /// <summary>
    /// Trigger a EndRound for every EntityComponent.
    /// </summary>
    public virtual void EndRound()
    {
        foreach (EntityComponent component in components)
        {
            component.EndRound();
        }
    }
}
