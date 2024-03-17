using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityActionDoer : Entity
{
    private Dictionary<Type, EntityActionComponent> actionComponents;

    public Dictionary<Type, EntityActionComponent> ActionComponents => actionComponents;

    protected override void SetComponents()
    {
        base.SetComponents();

        actionComponents = new Dictionary<Type, EntityActionComponent>();

        foreach (EntityComponent component in components)
        {
            if(component is  EntityActionComponent)
            {
                actionComponents.Add(component.GetType(), component as EntityActionComponent);
            }
        }
    }

    /// <summary>
    /// Try to get an EntityComponent by its type.
    /// </summary>
    /// <typeparam name="T">The type of the EntityComponent wanted.</typeparam>
    /// <param name="foundComponent">The found EntityComponent.</param>
    /// <returns>TRUE if an EntityComponent is found. Otherwise, FALSE.</returns>
    public bool TryGetActionComponentOfType<T>(out T foundComponent) where T : EntityActionComponent
    {
        foundComponent = null;

        if (actionComponents.ContainsKey(typeof(T)))
        {
            foundComponent = actionComponents[typeof(T)] as T;
        }

        return foundComponent != null;
    }
}
