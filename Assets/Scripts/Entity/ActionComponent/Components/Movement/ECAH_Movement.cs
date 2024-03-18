using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ECAH_Movement : PlayerEntityActionHandler<EC_Movement>
{
    private EC_Movement movementHandler => entityActionComponentHandled;

    [SerializeField] private CanvasGroup movementGroup;

    private void Update()
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
        {
            if (movementHandler.CanMove && !isLocked)
            {
                DisplayAction(InputManager.MousePosition);
            }
        }
    }

    public override void Enable()
    {
        movementGroup.alpha = 1f;
        movementGroup.interactable = true;
        movementGroup.blocksRaycasts = true;
    }

    public override void Disable()
    {
        movementGroup.alpha = 0f;
        movementGroup.interactable = false;
        movementGroup.blocksRaycasts = false;
    }

    public override void UpdateActionAvailibility()
    {
        if(movementHandler.CanMove)
        {
            movementGroup.interactable = !isLocked;
            movementGroup.blocksRaycasts = !isLocked;
        }
        else
        {
            movementGroup.interactable = false;
            movementGroup.blocksRaycasts = false;
        }
    }

    public override void SelectAction()
    {
        enabled = true;

        if (movementHandler.CanMove)
        {
            base.SelectAction();

            InputManager.Instance.OnMouseLeftDownWithoutObject += UseAction;
            InputManager.Instance.OnMouseLeftDownOnObject += UseActionOnInteractible;
        }
    }

    public override void UnselectAction()
    {
        enabled = false;
        UndisplayAction();

        InputManager.Instance.OnMouseLeftDownWithoutObject -= UseAction;
        InputManager.Instance.OnMouseLeftDownOnObject -= UseActionOnInteractible;
        base.UnselectAction();
    }

    protected override void DisplayAction(Vector3 actionTargetPosition)
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.RealTime || !movementHandler.CanMove)
        {
            return;
        }

        Color colorMovement = Color.green;
        colorMovement.a = 0.5f;
        GridZoneDisplayer.SetGridFeedback(Pathfinding.Instance.CalculatePathfinding(movementHandler.CurrentNode, null, movementHandler.MovementLeft), colorMovement);

        if (Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition) != null)
        {
            List<Node> path = Pathfinding.Instance.CalculatePathfinding(movementHandler.CurrentNode, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition), movementHandler.MovementLeft);

            List<Node> validPath = new List<Node>();
            List<Node> opportunityPath = new List<Node>();

            //bool foundOpportunityAttack = false;

            /*if (CheckForOpportunityAttack(currentNode))
            {
                foundOpportunityAttack = true;
            }*/

            foreach (Node n in path)
            {
                //if (!foundOpportunityAttack)
                {
                    /*if (CheckForOpportunityAttack(n))
                    {
                        foundOpportunityAttack = true;
                    }*/

                    validPath.Add(n);
                }
                /*else
                {
                    opportunityPath.Add(n);
                }*/
            }

            GridZoneDisplayer.SetGridFeedback(validPath, Color.green);
            GridZoneDisplayer.SetGridFeedback(opportunityPath, Color.red);
        }
    }

    protected override void UndisplayAction()
    {
        GridZoneDisplayer.UnsetGridFeedback();
    }

    /// <summary>
    /// Handle interaction.
    /// </summary>
    /// <param name="interactibleObject"></param>
    private void UseActionOnInteractible(EC_Clicable interactibleObject)
    {
        if(interactibleObject.HoldingEntity.TryGetComponentOfType<EC_Interactable>(out EC_Interactable interactibleComponent))
        {
            UseAction(interactibleComponent.HoldingEntity.transform.position, () => interactibleComponent.PlayInteraction(EndAction));
        }
        else
        {
            EndAction();
        }
    }

    public override void UseAction(Vector2 usePosition, Action callback)
    {
        UndisplayAction();

        base.UseAction(usePosition, callback);
    }

    protected override void EndAction()
    {
        base.EndAction();
    }
}
