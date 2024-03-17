using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityActionManager : Singleton<PlayerEntityActionManager>
{
    [SerializeField] private PlayerEntityActionHandler[] playerActions;

    [SerializeField] private Entity entityHandled;

    private PlayerEntityActionHandler selectedAction;

    private List<MonoBehaviour> locks = new List<MonoBehaviour>();

    public Entity EntityHandled => entityHandled;

    private void Start()
    {
        DisablePlayerActions();
    }

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

    private void DisablePlayerActions()
    {
        foreach(PlayerEntityActionHandler action in playerActions)
        {
            action.Disable();
        }
    }

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

    public void UpdateActionsAvailability()
    {
        foreach (PlayerEntityActionHandler action in playerActions)
        {
            action.UpdateActionAvailibility();
        }
    }

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

    public void UnselectAction()
    {
        selectedAction = null;
    }


    public void OnUseAction(EntityActionComponent actionUsed)
    {
        AddLock(this);
    }

    public void OnEndAction(EntityActionComponent actionEnded)
    {
        RemoveLock(this);
        UpdateActionsAvailability();
    }
}
