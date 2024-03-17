using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EC_Movement : EntityActionComponent<IEC_MovementData>
{
    [SerializeField] private float speed = 1;

    [SerializeField] private float movementByTurn = 15;

    [SerializeField] private Transform transformToMove;

    [Header("Feedbacks and Animations")]
    [SerializeField] private UnityEvent<Vector2> onChangeDirectionEvent;

    //Node
    private EC_NodeHandler nodeHandler;

    //Pathfinding
	private Node[] path = new Node[0];
    private int targetIndex;
    private Vector2 posUnit;
    private Vector2 posTarget;
    private Coroutine currentMovementRoutine;

	private float currentMovementLeft;

    //TODO : Interaction to do (On le fait ici ou dans un autre component ?)

    private Action endMovementCallback;

    private RoundMode currentRoundMode;

    public Action<Node> onEnterNode;
    public Action<Node> onExitNode;
    public Action onStartMovement;
    public Action onEndMovement;
    public Action onStopMovement;
    public Action onCancelMovement;

    public Node CurrentNode => nodeHandler.CurrentNode;

    public bool CanMove => currentMovementLeft >= Pathfinding.DirectDistance;

    public override void SetComponentData(IEC_MovementData componentData)
    {
        speed = componentData.Speed;
        movementByTurn = componentData.DistanceByTurn;
    }

    protected override void InitializeComponent()
    {
        if (!HoldingEntity.TryGetComponentOfType<EC_NodeHandler>(out nodeHandler))
        {
            Debug.LogError(HoldingEntity + " has EC_Movement without EC_NodeHandler.");
        }

        currentMovementLeft = movementByTurn;
    }

    public override void Activate()
    {
        if(RoundManager.Instance != null)
        {
            RoundManager.Instance.actOnUpdateRoundMode += ChangeRoundMode;

            ChangeRoundMode(RoundManager.Instance.CurrentRoundMode);
        }
        else
        {
            RoundManager.WaitForInitialization += () => RoundManager.Instance.actOnUpdateRoundMode += ChangeRoundMode;
        }
    }

    public override void Deactivate()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.actOnUpdateRoundMode -= ChangeRoundMode;
        }
    }

    public override void StartRound()
    {
        currentMovementLeft = movementByTurn;
    }

    public override void EndRound()
    {
        
    }

    private void ChangeRoundMode(RoundMode roundModeSet)
    {
        currentRoundMode = roundModeSet;

        if (nodeHandler != null)
        {
            CancelMovement();
        }
    }

    /// <summary>
    /// Try to move the Component to a target node.
    /// </summary>
    /// <param name="targetNode">The Node to reach.</param>
    /// <param name="movementCallback">The action to call once the movement reach the target Node.</param>
    public void TryMoveToDestination(Node targetNode, Action movementCallback)
    {
        TryMoveToDestination(targetNode.WorldPosition, movementCallback);
    }

    /// <summary>
    /// Try to move the Component to a target position.
    /// </summary>
    /// <param name="targetPosition">The position to reach.</param>
    /// <param name="movementCallback">The action to call once the movement reach the target position.</param>
    public void TryMoveToDestination(Vector3 targetPosition, Action movementCallback)
    {
        float possibleMovement = currentMovementLeft;

        if(currentRoundMode == RoundMode.RealTime)
        {
            possibleMovement = -1f;
        }

        if (Pathfinding.Instance.TryFindPath(CurrentNode.WorldPosition, targetPosition, possibleMovement, OnPathFound))
        {
            endMovementCallback = movementCallback;
        }
        else
        {
            movementCallback?.Invoke();
        }
    }

    /// <summary>
    /// Process the path that has been found.
    /// </summary>
    /// <param name="foundPath">The path that has been found.</param>
    private void OnPathFound(Node[] foundPath)
    {
        if(currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
            currentMovementRoutine = null;
        }

        path = foundPath;

        targetIndex = 0;

        StartMovement();
    }

    /// <summary>
    /// Start the Movement.
    /// </summary>
    private void StartMovement()
    {
        if (enabled)
        {
            onStartMovement?.Invoke();

            currentMovementRoutine = StartCoroutine(FollowPath());
        }
    }

    /// <summary>
    /// Cancel the current movement.
    /// </summary>
    private void CancelMovement()
    {
        //Animation ?

        onCancelMovement?.Invoke();

        //endMovementCallback?.Invoke(); //TODO : Voir comment on g�re pour les Interaction quand on fera le module d'interaction

        StopMovement();
    }

    /// <summary>
    /// End the current movement.
    /// </summary>
    private void EndMovement()
    {
        //Animation ?

        onEndMovement?.Invoke();

        endMovementCallback?.Invoke();

        StopMovement();
    }

    /// <summary>
    /// Stop the movement.
    /// </summary>
    /// <param name="hasBeenCanceled"></param>
    private void StopMovement()
    {
        //Animation

        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
            currentMovementRoutine = null;
        }

        transformToMove.position = CurrentNode.WorldPosition;

        endMovementCallback = null;

        onStopMovement?.Invoke();
    }

    /// <summary>
	/// Make the component follow the path.
	/// </summary>
	/// <returns></returns>
	private IEnumerator FollowPath()
    {
        Node currentWaypoint = path[targetIndex];

        //Animation

        float lerpValue = 0;
        float distance = 0;

        posUnit = new Vector2(transformToMove.position.x, transformToMove.position.y);
        posTarget = new Vector2(currentWaypoint.WorldPosition.x, currentWaypoint.WorldPosition.y);

        onChangeDirectionEvent?.Invoke(posTarget - posUnit); //TODO : Voir si on fait directement dans le code comme pour l'animation

        distance = Vector2.Distance(posUnit, posTarget);

        while (true)
        {
            lerpValue += Time.deltaTime * speed / distance;

            if (CurrentNode != currentWaypoint && lerpValue >= 1)
            {
                ChangeNode(currentWaypoint);
            }

            if (lerpValue >= 1)
            {
                targetIndex++;

                if (targetIndex >= path.Length)
                {
                    if (currentRoundMode == RoundMode.Round)
                    {
                        currentMovementLeft -= CurrentNode.gCost;
                    }

                    EndMovement();

                    transformToMove.position = currentWaypoint.WorldPosition;
                    break;
                }

                currentWaypoint = path[targetIndex];

                lerpValue--;

                posUnit = new Vector2(transformToMove.position.x, transformToMove.position.y);
                posTarget = new Vector2(currentWaypoint.WorldPosition.x, currentWaypoint.WorldPosition.y);

                onChangeDirectionEvent?.Invoke(posTarget - posUnit); //TODO : Voir si on fait directement dans le code comme pour l'animation

                distance = Vector2.Distance(posUnit, posTarget);

                //if (isMovementCosting || RVN_RoundManager.Instance.IsPaused) //TODO : Check pour les Cin�matique ou autres ?
                //{
                //    StopMovement();
                //}
            }

            transformToMove.position = Vector3.Lerp(posUnit, posTarget, lerpValue);
            yield return null;
        }
    }

    /// <summary>
    /// Teleport the Component to the target Node.
    /// </summary>
    /// <param name="teleportNode">The Node to teleport to.</param>
    public void Teleport(Node teleportNode)
    {
        ChangeNode(teleportNode);

        transformToMove.position = teleportNode.WorldPosition;
    }

    /// <summary>
    /// Change the Node on which the Component is.
    /// </summary>
    /// <param name="newNode">The new Node the Component is on.</param>
    private void ChangeNode(Node newNode)
    {
        onExitNode?.Invoke(CurrentNode);

        nodeHandler.SetNodeData(newNode);

        onEnterNode?.Invoke(CurrentNode);
    }

    /* ACTION SYSTEM
    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        if (CanMove && RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.RealTime)
        {
            return true;
        }

        Node toCheck = Grid.GetNodeFromWorldPoint(actionTargetPosition);

		if(toCheck.GetNodeComponent<CPN_InteractibleObject>().Count > 0)
		{
			if(Pathfinding.GetDistance(toCheck, CurrentNode) < 15)
			{
				return true;
			}
			else if(CanMove)
			{
				return CanInteractWithObject(toCheck.GetNodeComponent<CPN_InteractibleObject>()[0]);
			}
		}
		else
		{
            return CanMove && CanMoveToDestination(actionTargetPosition);
        }

		return false;
    }

    public override bool CanSelectAction()
    {
        return true;
    }

    public override void UnselectAction()
    {
		if (currentMovement != null)
		{
			StopMovement();
		}
    }

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        if (RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.RealTime)
        {
            return;
        }

        Color colorMovement = Color.green;
        colorMovement.a = 0.5f;
        RVN_GridDisplayer.SetGridFeedback(GetPossibleMovementTarget(), colorMovement);

        if (Grid.GetNodeFromWorldPoint(actionTargetPosition) != null)
        {
            List<Node> path = Pathfinding.CalculatePathfinding(currentNode, Grid.GetNodeFromWorldPoint(actionTargetPosition), currentMovementLeft);

            List<Node> validPath = new List<Node>();
            List<Node> opportunityPath = new List<Node>();

            bool foundOpportunityAttack = false;

            if (CheckForOpportunityAttack(currentNode))
            {
                foundOpportunityAttack = true;
            }

            foreach (Node n in path)
            {
                if (!foundOpportunityAttack)
                {
                    if (CheckForOpportunityAttack(n))
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

            RVN_GridDisplayer.SetGridFeedback(validPath, Color.green);
            RVN_GridDisplayer.SetGridFeedback(opportunityPath, Color.red);
        }
    }

    public override void UndisplayAction(Vector2 actionTargetPosition)
    {
        RVN_GridDisplayer.UnsetGridFeedback();
    }

    public override bool TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        AskToMoveTo(actionTargetPosition, callback);

		return true;
    }
     */

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i].WorldPosition, Vector3.one * 0.1f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transformToMove.position, path[i].WorldPosition);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1].WorldPosition, path[i].WorldPosition);
                }
            }
        }
    }

    public override bool IsActionAvailable()
    {
        return CanMove;
    }

    public override void SelectAction()
    {
        
    }

    public override void UnselectAction()
    {
       
    }

    public override void UndisplayAction()
    {
        GridZoneDisplayer.UnsetGridFeedback();
    }

    public override void DisplayAction(Vector3 actionTargetPosition)
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.RealTime || !CanMove)
        {
            return;
        }

        Color colorMovement = Color.green;
        colorMovement.a = 0.5f;
        GridZoneDisplayer.SetGridFeedback(Pathfinding.Instance.CalculatePathfinding(CurrentNode, null, currentMovementLeft), colorMovement);

        if (Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition) != null)
        {
            List<Node> path = Pathfinding.Instance.CalculatePathfinding(CurrentNode, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition), currentMovementLeft);

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

    public override bool IsActionUsable(Vector3 positionToCheck)
    {
        return true;
    }

    protected override void OnUseAction(Vector3 actionPosition)
    {
        TryMoveToDestination(actionPosition, EndAction);
    }

    public override void CancelAction()
    {
        CancelMovement();
    }

    protected override void OnEndAction()
    {
        
    }
}