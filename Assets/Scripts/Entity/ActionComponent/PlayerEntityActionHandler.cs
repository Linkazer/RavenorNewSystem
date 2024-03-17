using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityActionHandler : MonoBehaviour
{
    [SerializeField] private PlayerEntityActionInput[] playerActions;

    [SerializeField] private EntityActionDoer actionDoerHandled;

    public EntityActionDoer ActionDoerHandled => actionDoerHandled;

    public void SelectEntityActionDoer(EntityActionDoer actionDoer)
    {
        if(actionDoerHandled != actionDoer)
        {
            actionDoerHandled = actionDoer;

            foreach (PlayerEntityActionInput action in playerActions)
            {
                action.SetHandler(this);
            }
        }
    }

    private void EnablePlayerActions()
    {
        foreach (PlayerEntityActionInput action in playerActions)
        {
            if(actionDoerHandled.ActionComponents.ContainsKey(action.GetEntityActionType))
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
        foreach(PlayerEntityActionInput action in playerActions)
        {
            action.Disable();
        }
    }

    //Update doable action (Changement perso, fin d'action)

    //On fin d'action

    //Do selected action ? (Géré directement dans les ActionInput ?)
    // Si on fait dans les ActionInput, ajouter une fonction pour dire qu'une action est en cours

    //Display selected action
}
