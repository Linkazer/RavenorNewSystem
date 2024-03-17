using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains all the data needed by a Node.
/// </summary>
public class EC_NodeHandler : EntityComponent<IEC_GridEntityData>
{
    /// <summary>
    /// Does the component block the vision.
    /// </summary>
    [SerializeField] private bool blockVision = false;
    /// <summary>
    /// Can the component be walk on.
    /// </summary>
    [SerializeField] private bool walkable = true;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent<Node> OnEnterNode;          // Played when the component enter a node.
    [SerializeField] private UnityEvent<Node> OnExitNode;           // Player when the component exit a node.
    [SerializeField] private UnityEvent<EC_NodeHandler> OnDataEnter; // Played when an other component enter the node on which the component is.
    [SerializeField] private UnityEvent<EC_NodeHandler> OnDataExit;  // Played when an other component exit the node on which the component is.

    [Header("Devs")]
    [SerializeField] private bool drawGizmos = false;

    public Action<EC_NodeHandler> actOnDataEnter;
    public Action<EC_NodeHandler> actOnDataExit;

    /// <summary>
    /// The node on which the component is.
    /// </summary>
    private Node currentNode = null;

    public Node CurrentNode => currentNode;

    public bool Walkable => walkable;

    public bool BlockVision => blockVision;

    public override void SetComponentData(IEC_GridEntityData componentData)
    {
        SetWalkable(componentData.Walkable);
        SetBlockingVision(componentData.BlockVision);
    }

    protected override void InitializeComponent()
    {
        SetNodeDataFromPosition();
    }

    public void SetWalkable(bool toSet)
    {
        walkable = toSet;
    }

    public void SetBlockingVision(bool toSet)
    {
        blockVision = toSet;
    }

    public override void Activate()
    {
        SetNodeDataFromPosition();
    }

    public override void Deactivate()
    {
        UnsetNodeData(false);
    }

    public override void StartRound()
    {
        
    }

    public override void EndRound()
    {
        
    }

    /// <summary>
    /// Assign a new node to the component.
    /// </summary>
    /// <param name="nNode">The node to assign.</param>
    public void SetNodeData(Node nNode)
    {
        UnsetNodeData(true);

        currentNode = nNode;

        if (currentNode != null)
        {
            currentNode.AddEntityOnNode(this);

            OnEnterNode?.Invoke(currentNode);
        }
    }

    /// <summary>
    /// Assign a new node to the component depending on the position of the GameObject.
    /// </summary>
    public void SetNodeDataFromPosition()
    {
        SetNodeData(Grid.Instance.GetNodeFromWorldPoint(transform.position));
    }

    /// <summary>
    /// Unassign the current node.
    /// </summary>
    public void UnsetNodeData(bool applyNodeEffect)
    {
        if (currentNode != null)
        {
            if (applyNodeEffect)
            {
                OnExitNode?.Invoke(currentNode);
            }

            currentNode.RemoveEntityOnNode(this);

            currentNode = null;
        }
    }

    public bool TryGetEntityComponentFromHoldingEntity<T>(out T foundComponent) where T : EntityComponent
    {
        if(HoldingEntity == null)
        {
            foundComponent = null;
            return false;
        }

        return HoldingEntity.TryGetComponentOfType<T>(out foundComponent);
    }

    /// <summary>
    /// Called when an other component enter the current node.
    /// </summary>
    /// <param name="dataEnter">The NodeDataHandler that enter the node.</param>
    public void OnDataEnterCurrentNode(EC_NodeHandler dataEnter)
    {
        OnDataEnter?.Invoke(dataEnter);
        actOnDataEnter?.Invoke(dataEnter);
    }
    /// <summary>
    /// Called when an other component exit the current node.
    /// </summary>
    /// <param name="dataExit">The NodeDataHandler that exit the node.</param>
    public void OnDataExitCurrentNode(EC_NodeHandler dataExit)
    {
        OnDataExit?.Invoke(dataExit);
        actOnDataExit?.Invoke(dataExit);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
#endif
}
