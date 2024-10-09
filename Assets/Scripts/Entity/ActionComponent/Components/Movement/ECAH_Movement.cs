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

    [SerializeField] private Color mouseTargetOutlineColor;

    private List<EC_Renderer> rendererOutlined = new List<EC_Renderer>();

    private void Update()
    {
        if (!isLocked)
        {
            DisplayAction(InputManager.MousePosition);
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

    //Called by the UI Button
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

    protected override void DisplayAction(Vector3? actionTargetPosition)
    {
        UndisplayAction();

        if (RoundManager.Instance.CurrentRoundMode == RoundMode.RealTime || !movementHandler.CanMove)
        {
            return;
        }

        Color colorMovement = Color.green;
        colorMovement.a = 0.5f;
        GridZoneDisplayer.SetGridFeedback(Pathfinding.Instance.CalculatePathfinding(movementHandler.CurrentNode, null, movementHandler.MovementLeft), colorMovement);

        if (actionTargetPosition != null)
        {
            Node targetNode = Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition.Value);

            if (targetNode != null)
            {
                List<EC_HealthHandler> targetablesInZone = targetNode.GetEntityComponentsOnNode<EC_HealthHandler>();

                foreach (EC_HealthHandler healthHandler in targetablesInZone)
                {
                    if (healthHandler.HoldingEntity.TryGetEntityComponentOfType(out EC_Renderer rend))
                    {
                        rend.AnimHandler.SetOutline(mouseTargetOutlineColor);

                        if (!rendererOutlined.Contains(rend))
                        {
                            rendererOutlined.Add(rend);
                        }
                    }
                }
            }

            if (targetNode != null)
            {
                List<Node> path = Pathfinding.Instance.CalculatePathfinding(movementHandler.CurrentNode, targetNode, movementHandler.MovementLeft);

                List<Node> validPath = new List<Node>();
                List<Node> opportunityPath = new List<Node>();

                bool foundOpportunityAttack = false;

                if (movementHandler.CheckForOpportunityAttack(movementHandler.CurrentNode, false))
                {
                    foundOpportunityAttack = true;
                }

                foreach (Node n in path)
                {
                    if (!foundOpportunityAttack)
                    {
                        if (movementHandler.CheckForOpportunityAttack(n, false))
                        {
                            foundOpportunityAttack = true;
                        }

                        validPath.Add(n);
                    }
                    else
                    {
                        opportunityPath.Add(n);
                    }
                }

                GridZoneDisplayer.SetGridFeedback(validPath, Color.green);
                GridZoneDisplayer.SetGridFeedback(opportunityPath, Color.red);
            }
        }
    }

    protected override void UndisplayAction()
    {
        foreach (EC_Renderer renderedOutline in rendererOutlined)
        {
            renderedOutline.AnimHandler.SetOutline(false);
        }
        rendererOutlined.Clear();

        GridZoneDisplayer.UnsetGridFeedback();
    }

    /// <summary>
    /// Handle interaction.
    /// </summary>
    /// <param name="interactibleObject"></param>
    private void UseActionOnInteractible(EC_Clicable interactibleObject)
    {
        if(interactibleObject.HoldingEntity.TryGetEntityComponentOfType<EC_Interactable>(out EC_Interactable interactibleComponent))
        {
            UseAction(interactibleComponent.HoldingEntity.transform.position, () => interactibleComponent.PlayInteraction(EndAction, movementHandler.HoldingEntity));
        }
        else
        {
            EndAction();
        }
    }

    public override void UseAction(Vector2 usePosition, Action callback)
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.RealTime && ControllableTeamHandler.Instance.AreCharacterGrouped)
        {
            foreach (CharacterEntity chara in ControllableTeamHandler.Instance.CurrentControllableCharacters)
            {
                if (chara.TryGetEntityComponentOfType(out EC_NodeHandler nodeHandler))
                {
                    nodeHandler.SetWalkable(true);
                }
            }

            base.UseAction(usePosition, callback);

            MoveAllOtherEntities(usePosition);
        }
        else
        {
            base.UseAction(usePosition, callback);
        }
    }

    protected override void EndAction()
    {
        foreach (CharacterEntity chara in ControllableTeamHandler.Instance.CurrentControllableCharacters)
        {
            if (chara.TryGetEntityComponentOfType(out EC_NodeHandler nodeHandler))
            {
                nodeHandler.SetWalkable(false);
            }
        }

        base.EndAction();
    }


    private void MoveAllOtherEntities(Vector2 mainCharacterTargetPosition)
    {
        int currentPathIndex = 0;

        Node[] targetNodes = GetFollowerPositions(movementHandler.CurrentMovementTarget, movementHandler.CurrentMovementBeforeTarget, ControllableTeamHandler.Instance.CurrentControllableCharacters.Count);

        foreach (CharacterEntity chara in ControllableTeamHandler.Instance.CurrentControllableCharacters)
        {
            if (chara.TryGetEntityComponentOfType(out EC_Movement allyMovement))
            {
                if (allyMovement != movementHandler && currentPathIndex < targetNodes.Length)
                {
                    allyMovement.UseAction(targetNodes[currentPathIndex].WorldPosition, null);
                    currentPathIndex++;
                }
            }
        }
    }

    private Node[] GetFollowerPositions(Node targetNode, Node beforeTargetNode, int numberPositionWanted)
    {
        Vector2 direction = new Vector2(beforeTargetNode.GridX - targetNode.GridX, beforeTargetNode.GridY - targetNode.GridY);

        Node[] toReturn = new Node[numberPositionWanted];
        List<Node> usedNodes = new List<Node>();
        usedNodes.Add(targetNode);

        List<Node> usableNodes = GetFollowerNodes(direction, targetNode, numberPositionWanted);
        for (int i = 0; i < numberPositionWanted; i++)
        {
            toReturn[i] = usableNodes[i];
        }

        return toReturn;
    }

    private List<Node> GetFollowerNodes(Vector2 direction, Node startNode, int numberPositionWanted)
    {
        List<Node> allNodeInDistance = Pathfinding.Instance.GetAllNodeInDistance(startNode, numberPositionWanted * 10, false);

        allNodeInDistance.Remove(startNode);

        for (int i = 0; i < allNodeInDistance.Count; i++)
        {
            if (!allNodeInDistance[i].IsWalkable)// && direction.x != 0 && ((allNodeInDistance[i].gridX - startNode.gridX) * direction.x > 0) && ((allNodeInDistance[i].gridY - startNode.gridY) *direction.y > 0))
            {
                allNodeInDistance.RemoveAt(i);
                i--;
            }
        }

        allNodeInDistance.Sort((n1, n2) => CompareNodes(n1, n2, startNode, direction));

        return allNodeInDistance;
    }

    private int CompareNodes(Node n1, Node n2, Node startNode, Vector2 direction)
    {
        float n1Score = 0, n2Score = 0;

        n1Score = Pathfinding.Instance.GetDistance(startNode, n1);
        if (direction.x != 0 && ((n1.GridX - startNode.GridX) * direction.x < 0))
        {
            n1Score += 50;
        }
        else if (direction.y == 0 && ((n1.GridX - startNode.GridX) * direction.x > 0))
        {
            n1Score -= 25;
        }
        if (direction.y != 0 && ((n1.GridY - startNode.GridY) * direction.y < 0))
        {
            n1Score += 50;
        }
        else if (direction.x == 0 && ((n1.GridY - startNode.GridY) * direction.y > 0))
        {
            n1Score -= 25;
        }
        if (!Pathfinding.Instance.IsNodeVisible(startNode, n1))
        {
            n1Score += 100;
        }

        n2Score = Pathfinding.Instance.GetDistance(startNode, n2);
        if (direction.x != 0 && ((n2.GridX - startNode.GridX) * direction.x < 0))
        {
            n2Score += 50;
        }
        else if (direction.y == 0 && ((n2.GridX - startNode.GridX) * direction.x > 0))
        {
            n2Score -= 25;
        }
        if (direction.y != 0 && ((n2.GridY - startNode.GridY) * direction.y < 0))
        {
            n2Score += 50;
        }
        else if (direction.x == 0 && ((n2.GridY - startNode.GridY) * direction.y > 0))
        {
            n2Score -= 25;
        }
        if (!Pathfinding.Instance.IsNodeVisible(startNode, n2))
        {
            n2Score += 100;
        }

        if (n1Score > n2Score)
        {
            return 1;
        }
        else if (n1Score < n2Score)
        {
            return -1;
        }

        return 0;
    }

}
