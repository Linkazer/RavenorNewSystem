using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionManager : Singleton<PlayerActionManager>
{
    [SerializeField] private PlayerEntityComponentDisplay[] playerDisplays;

    [SerializeField] private PlayerActionHandler[] playerActions;

    [SerializeField] private PlayerEntityActionHandler[] playerEntityActions;

    private Entity entityHandled;

    private PlayerEntityActionHandler selectedAction;

    [SerializeField] private List<MonoBehaviour> locks = new List<MonoBehaviour>();

    public Entity EntityHandled => entityHandled;

    private void Start()
    {
        DisablePlayerActions();

        foreach(PlayerActionHandler action in playerActions)
        {
            action.SetHandler(this);
        }
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

            if(entityHandled != null)
            {
                foreach (PlayerEntityComponentDisplay display in playerDisplays)
                {
                    display.SetCharacter(null);
                }
            }

            entityHandled = entity;

            if (entityHandled == null)
            {
                foreach (PlayerEntityActionHandler action in playerEntityActions)
                {
                    action.UnsetHandler();
                }

                DisablePlayerActions();
            }
            else
            {
                foreach (PlayerEntityActionHandler action in playerEntityActions)
                {
                    action.SetHandler(this);
                }

                foreach(PlayerEntityComponentDisplay display in playerDisplays)
                {
                    display.SetCharacter(entityHandled as CharacterEntity);
                }

                CameraController.Instance.SetCameraFocus(entityHandled.transform);

                EnablePlayerActions();
                if (selectedAction == null)
                {
                    playerEntityActions[0].SelectAction();
                }
            }
        }
    }

    /// <summary>
    /// Enable every PlayerEntityActionHandler that the controlled entity has.
    /// </summary>
    private void EnablePlayerActions()
    {
        foreach (PlayerEntityActionHandler action in playerEntityActions)
        {
            if(action.GetEntityActionType == null || entityHandled.ComponentsByType.ContainsKey(action.GetEntityActionType))
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
        foreach(PlayerEntityActionHandler action in playerEntityActions)
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
        //Debug.Log("Add lock : " + lockCaller);

        locks.Add(lockCaller);

        if(locks.Count == 1)
        {
            foreach (PlayerEntityActionHandler action in playerEntityActions)
            {
                action.Lock(true);
            }

            foreach(PlayerActionHandler playerAction in playerActions)
            {
                playerAction.Lock(true);
            }
        }
    }

    /// <summary>
    /// Remove a Lock on the player action.
    /// </summary>
    /// <param name="unlockCaller">The caller of the lock to remove.</param>
    public void RemoveLock(MonoBehaviour unlockCaller)
    {
        //Debug.Log("Try remove lock : " + unlockCaller);

        if (locks.Contains(unlockCaller))
        {
            //Debug.Log("Remove lock : " + unlockCaller);

            locks.Remove(unlockCaller);

            if (locks.Count == 0)
            {
                foreach (PlayerEntityActionHandler action in playerEntityActions)
                {
                    action.Lock(false);
                }

                foreach (PlayerActionHandler playerAction in playerActions)
                {
                    playerAction.Lock(false);
                }
            }
        }
    }

    /// <summary>
    /// Update every PlayerEntityActionHandler
    /// </summary>
    public void UpdateActionsAvailability()
    {
        foreach (PlayerEntityActionHandler action in playerEntityActions)
        {
            action.UpdateActionAvailibility();
        }
    }

    /// <summary>
    /// Select an Action.
    /// </summary>
    /// <param name="actionToSelect">The action to select.</param>
    public void OnSelectAction(PlayerEntityActionHandler actionToSelect)
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
    public void OnUnselectAction()
    {
        if (selectedAction == playerEntityActions[0])
        {
            selectedAction = null;
        }
        else
        {
            selectedAction = playerEntityActions[0];
            playerEntityActions[0].SelectAction();
        }
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
