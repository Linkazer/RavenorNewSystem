using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IRoundHandler
{
    [SerializeField] private bool activateOnStart = true;
    [SerializeField] protected EntityComponent[] components;

    private Dictionary<Type, EntityComponent> componentByType = new Dictionary<Type, EntityComponent>();

    public Action actOnEntityStartRound;
    public Action actOnEntityEndRound;

    public Action<Entity> actOnActivateEntity;
    public Action<Entity> actOnDeactivateEntity;

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
    public bool TryGetEntityComponentOfType<T>(out T foundComponent) where T : EntityComponent
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

        if (activateOnStart)
        {
            Activate();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Set Component")]
    public void EDITOR_SetComponents()
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

            component.EDITOR_SetComponents(this);
        }
    }
#endif

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
    public virtual void Activate()
    {
        foreach (EntityComponent component in components)
        {
            component.Activate();
        }

        actOnActivateEntity?.Invoke(this);

        RoundManager.Instance.AddHandler(this);
    }

    /// <summary>
    /// Deactivate every EntityComponent.
    /// </summary>
    public virtual void Deactivate()
    {
        RoundManager.Instance.RemoveHandler(this);

        actOnDeactivateEntity?.Invoke(this);

        foreach (EntityComponent component in components)
        {
            component.Deactivate();
        }

        gameObject.SetActive(false);//TEST
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

        actOnEntityStartRound?.Invoke();
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

        actOnEntityEndRound?.Invoke();
    }
}
