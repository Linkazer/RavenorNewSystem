using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IHeapItem<Node> 
{
	public delegate bool NodeBlocker(EC_NodeHandler handlerToBlock, Action callback, object[] triggerData);

	//Data Handlers
	private List<EC_NodeHandler> entitiesOnNode = new List<EC_NodeHandler>();

	public List<NodeBlocker> entryBlockers = new List<NodeBlocker>();
	public List<NodeBlocker> exitBlockers = new List<NodeBlocker>();

	//Data
	private GridElement linkedElement;
	private bool staticObstacle;
    private Vector3 worldPosition;
    private int gridX;
    private int gridY;

	//Pathfinding
    public float gCost;
    public float hCost;
    public Node parent;
    public Node children;

	public bool IsStaticObstacle => staticObstacle;
	public int GridX => gridX;
	public int GridY => gridY;
	public Vector3 WorldPosition => linkedElement?linkedElement.EntityPosition:worldPosition;

	public GridElement LinkedElement => linkedElement;

    public bool IsWalkable => !staticObstacle && CheckWalkableFromEntitiesOnNode();

    public bool IsVisible => !staticObstacle && CheckVisibleFromEntitiesOnNode();

    public List<EC_NodeHandler> EntitiesOnNode => entitiesOnNode;


    public Node(bool _walkable, GridElement element, Vector3 _worldPos, int _gridX, int _gridY) {

		SetNode(_walkable, element, _worldPos, _gridX, _gridY);
	}

    /// <summary>
    /// Set the static data of the Node.
    /// </summary>
    /// <param name="_walkable">Is the Node walkable.</param>
    /// <param name="_worldPos">The Node World Position.</param>
    /// <param name="_gridX">The Node grid position on X axis.</param>
    /// <param name="_gridY">The Node grid position on Y axis.</param>
    public void SetNode(bool _walkable, GridElement element, Vector3 _worldPos, int _gridX, int _gridY)
	{
		linkedElement = element;

		staticObstacle = !_walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;

		gCost = 0;
		hCost = 0;
		parent = null;
		children = null;
		heapIndex = 0;
	}

	/// <summary>
	/// Check if there are any Data On Node that is not walkable.
	/// </summary>
	/// <returns></returns>
	private bool CheckWalkableFromEntitiesOnNode()
    {
		bool toReturn = true;
		
		for(int i = 0; i < entitiesOnNode.Count; i++)
        {
			if(!entitiesOnNode[i].Walkable)
            {
				toReturn = false;
            }
        }

		return toReturn;
    }

	/// <summary>
	/// Check if there are any Data On Node that blobk the vision.
	/// </summary>
	/// <returns></returns>
	private bool CheckVisibleFromEntitiesOnNode()
    {
		bool toReturn = true;

		for (int i = 0; i < entitiesOnNode.Count; i++)
		{
			if (entitiesOnNode[i].BlockVision)
			{
				toReturn = false;
			}
		}

		return toReturn;
	}

	/// <summary>
	/// Add a NodeDataHandler on the Node.
	/// </summary>
	/// <param name="toAdd">The NodeDataHandler to add.</param>
	public void AddEntityOnNode(EC_NodeHandler toAdd)
	{
		if (!entitiesOnNode.Contains(toAdd))
		{
			for(int i = 0; i < entitiesOnNode.Count; i++)
            {
				entitiesOnNode[i].OnDataEnterCurrentNode(toAdd);
			}

			entitiesOnNode.Add(toAdd);
		}
	}

	/// <summary>
	/// Remove a NodeDataHandler from the Node.
	/// </summary>
	/// <param name="toRemove">The NodeDataHandler to remove.</param>
	public void RemoveEntityOnNode(EC_NodeHandler toRemove)
    {
		if (entitiesOnNode.Contains(toRemove))
		{
			entitiesOnNode.Remove(toRemove);

			for (int i = 0; i < entitiesOnNode.Count; i++)
			{
				entitiesOnNode[i].OnDataExitCurrentNode(toRemove);
			}
		}
	}

	/// <summary>
	/// Get a every Component of a type on the Node.
	/// </summary>
	/// <typeparam name="T">The type of Component to search for.</typeparam>
	/// <returns>A list of every Component found.</returns>
	public List<T> GetEntityComponentsOnNode<T>() where T : EntityComponent
	{
        List<T> toReturn = new List<T>();

        for (int i = 0; i < entitiesOnNode.Count; i++)
        {
            if (entitiesOnNode[i].TryGetEntityComponentFromHoldingEntity(out T foundComponent))
            {
                toReturn.Add(foundComponent);
            }
        }
        return new List<T>(toReturn);
    }

    #region Heap
    int heapIndex;

    public float fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
    #endregion
}
