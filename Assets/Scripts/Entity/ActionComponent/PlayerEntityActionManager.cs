using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityActionManager : Singleton<PlayerEntityActionManager>
{
    [SerializeField] private PlayerEntityActionHandler[] playerActions;

    [SerializeField] private Entity entityHandled;

    private PlayerEntityActionHandler selectedAction;

    [SerializeField] private List<MonoBehaviour> locks = new List<MonoBehaviour>();

    public Entity EntityHandled => entityHandled;

    private void Start()
    {
        DisablePlayerActions();
    }

    /// <summary>
    /// Select the entity that is controlled.
    /// </summary>
    /// <param name="entity">The controlled entity.</param>
    public void SelectEntity(Entity entity)
    {
        if(entityHandled != entity)
        {
            if(selectedAction != null)
            {
                if(locks.Contains(this))
                {
                    OnEndAction(null);
                }
                selectedAction.UnselectAction();
            }

            entityHandled = entity;

            foreach (PlayerEntityActionHandler action in playerActions)
            {
                action.SetHandler(this);
            }

            if (entityHandled == null)
            {
                DisablePlayerActions();
            }
            else
            {
                EnablePlayerActions();
            }
        }
    }

    /// <summary>
    /// Enable every PlayerEntityActionHandler that the controlled entity has.
    /// </summary>
    private void EnablePlayerActions()
    {
        foreach (PlayerEntityActionHandler action in playerActions)
        {
            if(entityHandled.ComponentsByType.ContainsKey(action.GetEntityActionType))
            {
                action.Enable();
            }
            else
            {
                action.Disable();
            }
        }
    }

    /// <summary>
    /// Disable every PlayerEntityActionHandler
    /// </summary>
    private void DisablePlayerActions()
    {
        foreach(PlayerEntityActionHandler action in playerActions)
        {
            action.Disable();
        }
    }

    /// <summary>
    /// Add a Lock on the player action, preventing him to control the entity.
    /// </summary>
    /// <param name="lockCaller">The caller of the Lock.</param>
    public void AddLock(MonoBehaviour lockCaller)
    {
        locks.Add(lockCaller);

        if(locks.Count == 1)
        {
            foreach (PlayerEntityActionHandler action in playerActions)
            {
                action.Lock(true);
            }
        }
    }

    /// <summary>
    /// Remove a Lock on the player action.
    /// </summary>
    /// <param name="unlockCaller">The caller of the lock to remove.</param>
    public void RemoveLock(MonoBehaviour unlockCaller)
    {
        if (locks.Contains(unlockCaller))
        {
            locks.Remove(unlockCaller);

            if (locks.Count == 0)
            {
                foreach (PlayerEntityActionHandler action in playerActions)
                {
                    action.Lock(false);
                }
            }
        }
    }

    /// <summary>
    /// Update every PlayerEntityActionHandler
    /// </summary>
    public void UpdateActionsAvailability()
    {
        foreach (PlayerEntityActionHandler action in playerActions)
        {
            action.UpdateActionAvailibility();
        }
    }

    /// <summary>
    /// Select an Action.
    /// </summary>
    /// <param name="actionToSelect">The action to select.</param>
    public void SelectAction(PlayerEntityActionHandler actionToSelect)
    {
        if(selectedAction != actionToSelect)
        {
            if (selectedAction != null)
            {
                selectedAction.UnselectAction();
            }
            selectedAction = actionToSelect;
        }
    }

    /// <summary>
    /// Unselect the current action.
    /// </summary>
    public void UnselectAction()
    {
        selectedAction = null;
    }

    /// <summary>
    /// Called when an action is used.
    /// </summary>
    /// <param name="actionUsed">The action used.</param>
    public void OnUseAction(EntityActionComponent actionUsed)
    {
        //Debug.Log("Use action : " + actionUsed);
    }

    /// <summary>
    /// Called when an action ends.
    /// </summary>
    /// <param name="actionEnded">The action that ends.</param>
    public void OnEndAction(EntityActionComponent actionEnded)
    {
        //Debug.Log("End action : " + actionEnded);
        UpdateActionsAvailability();
    }
}
