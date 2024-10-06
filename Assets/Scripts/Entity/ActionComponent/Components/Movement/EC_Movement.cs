using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EC_Movement : EntityActionComponent<IEC_MovementData>
{
    [SerializeField] private float speed = 1;

    [SerializeField] private float movementByTurn = 15;

    [SerializeField] private Transform transformToMove;

    //Node
    private EC_NodeHandler nodeHandler;

    //Pathfinding
	private Node[] path = new Node[0];
    private int targetIndex;
    private Vector3 posUnit;
    private Vector3 posTarget;
    private Coroutine currentMovementRoutine;

	private float currentMovementLeft;
    private Action endMovementCallback;

    private RoundMode currentRoundMode;

    private EC_Renderer entityAnimator;

    private int opportunityAttackLeft;

    public Action<Node> onEnterNode;
    public Action<Node> onExitNode;
    public Action onStartMovement;
    public Action onEndMovement;
    public Action onStopMovement;
    public Action onCancelMovement;

    public Node CurrentNode => nodeHandler.CurrentNode;

    public float MovementLeft => currentMovementLeft;

    public Node[] CurrentPath => path;

    public Node CurrentMovementTarget => path.Length > 0 ? path[path.Length - 1] : CurrentNode;
    public Node CurrentMovementBeforeTarget => path.Length > 1 ? path[path.Length - 2] : CurrentNode;

    public bool CanMove => currentMovementLeft >= Pathfinding.DirectDistance;

    public override void SetComponentData(IEC_MovementData componentData)
    {
        speed = componentData.Speed;
        movementByTurn = componentData.DistanceByTurn;
    }

    protected override void InitializeComponent()
    {
        if (!HoldingEntity.TryGetEntityComponentOfType<EC_NodeHandler>(out nodeHandler))
        {
            Debug.LogError(HoldingEntity + " has EC_Movement without EC_NodeHandler.");
        }

        if(!HoldingEntity.TryGetEntityComponentOfType<EC_Renderer>(out entityAnimator))
        {
            Debug.LogError(HoldingEntity + " has no animator");
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
            path = new Node[0];
            movementCallback?.Invoke();
        }
    }

    public void MoveAlongPath(Node[] pathToMoveAlong)
    {
        OnPathFound(pathToMoveAlong);
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
        onCancelMovement?.Invoke();

        StopMovement();
    }

    /// <summary>
    /// End the current movement.
    /// </summary>
    private void EndMovement()
    {
        path = new Node[0];

        Action movementCallback = endMovementCallback;

        StopMovement();

        onEndMovement?.Invoke();

        movementCallback?.Invoke();
    }

    /// <summary>
    /// Stop the movement.
    /// </summary>
    /// <param name="hasBeenCanceled"></param>
    private void StopMovement()
    {
        entityAnimator?.AnimHandler.EndAnimation();

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

        float lerpValue = 0;
        float distance = 0;

        posUnit = new Vector3(transformToMove.position.x, transformToMove.position.y, transform.position.z);
        posTarget = new Vector3(currentWaypoint.WorldPosition.x, currentWaypoint.WorldPosition.y, currentWaypoint.WorldPosition.z);

        distance = Vector3.Distance(posUnit, posTarget);

        entityAnimator?.SetOrientation(posTarget - posUnit);

        if(CheckForOpportunityAttack(CurrentNode, true))
        {
            EndMovement();
            yield break;
        }

        entityAnimator?.AnimHandler.PlayAnimation("Walk", null);

        while (true)
        {
            lerpValue += Time.deltaTime * speed / distance;

            if (lerpValue >= 1) //Check for the next Node to reach
            {
                if(CurrentNode != currentWaypoint)
                {
                    if (currentRoundMode == RoundMode.Round)
                    {
                        currentMovementLeft -= Pathfinding.Instance.GetDistance(CurrentNode, currentWaypoint);
                    }

                    ChangeNode(currentWaypoint);
                }

                targetIndex++;

                if (targetIndex >= path.Length)//Check if the movement should end.
                {
                    EndMovement();

                    transformToMove.position = currentWaypoint.WorldPosition;
                    break;
                }
                else
                {
                    if (CheckForOpportunityAttack(CurrentNode, true))
                    {
                        EndMovement();
                        break;
                    }

                    //Continue following path
                    currentWaypoint = path[targetIndex];

                    lerpValue--;

                    posUnit = new Vector3(transformToMove.position.x, transformToMove.position.y, transform.position.z);
                    posTarget = new Vector3(currentWaypoint.WorldPosition.x, currentWaypoint.WorldPosition.y, currentWaypoint.WorldPosition.z);

                    entityAnimator?.SetOrientation(posTarget - posUnit);

                    distance = Vector3.Distance(posUnit, posTarget);
                }
            }

            transformToMove.position = Vector3.Lerp(posUnit, posTarget, lerpValue);
            yield return null;
        }
    }

    public bool CheckForOpportunityAttack(Node nodeToCheckAround, bool triggerAttack)
    {
        bool toReturn = false;

        object[] blockerTriggerData = new object[] { triggerAttack };

        foreach (Node.NodeBlocker blocker in nodeToCheckAround.exitBlockers)
        {
            if (blocker.Invoke(nodeHandler, TriggeredOpportunityAttackEnded, blockerTriggerData))
            {
                opportunityAttackLeft++;
                toReturn = true;
            }
        }

        return toReturn;
    }

    private void TriggeredOpportunityAttackEnded()
    {
        opportunityAttackLeft--;

        if(opportunityAttackLeft <= 0)
        {
            EndMovement();
        }
    }

    /*private bool TryTriggerOpportunityAttack(Node nodeToExit)
    {
        bool toReturn = false;

        object[] blockerTriggerData = new object[] { true };

        foreach(Node.NodeBlocker blocker in nodeToExit.exitBlockers)
        {
            if(blocker.Invoke(nodeHandler, blockerTriggerData))
            {
                toReturn = true;
            }
        }

        return toReturn;
    }*/

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

    public override bool IsActionAvailable()
    {
        return CanMove;
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

    public void OnDrawGizmos()
    {
        if (path != null && path.Length > 0)
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

}
