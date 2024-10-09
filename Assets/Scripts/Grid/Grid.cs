﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Rendering.FilterWindow;

public class Grid : Singleton<Grid> 
{
	[SerializeField] private bool displayGridGizmos;
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;

	private float nodeDiameter => nodeRadius * 2f;
	private int gridSizeX, gridSizeY;
    float minXPosition;
    float minYPosition;
    float maxXPosition;
    float maxYPosition;

    [SerializeField] private GridZoneDisplayer gridDisplayer;

	[Header("Generated Grid")]
	[SerializeField] private GridElement[] levelGrid;
    [SerializeField] private Node[,] grid;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
    }

	public void InitializeGrid()
	{
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

	/// <summary>
	/// Create the Grid.
	/// </summary>
	[ContextMenu("GenerateGrid")]
	public void CreateGrid() 
	{
		Dictionary<Vector2, GridElement> generatedNodes = new Dictionary<Vector2, GridElement>();

		minXPosition = levelGrid[0].WorldPosition.x;
		minYPosition = levelGrid[0].WorldPosition.y;
        maxXPosition = levelGrid[0].WorldPosition.x;
        maxYPosition = levelGrid[0].WorldPosition.y;

		int elementIndex = 0;

        foreach (GridElement element in levelGrid)
		{
			elementIndex++;

            element.gameObject.name = "Grid Element " + element.FlatPosition;


            if (generatedNodes.ContainsKey(element.FlatPosition))
			{
				Debug.LogError("Found 2 GridElements at position " + element.FlatPosition);
				continue;
			}

			generatedNodes.Add(element.FlatPosition,element);

			if(element.WorldPosition.x < minXPosition)
			{
				minXPosition = element.WorldPosition.x;
			}

			if(element.WorldPosition.y < minYPosition)
			{
				minYPosition = element.WorldPosition.y;
			}

			if (element.WorldPosition.x > maxXPosition)
			{
				maxXPosition = element.WorldPosition.x;
			}

            if (element.WorldPosition.y > maxYPosition)
            {
                maxYPosition = element.WorldPosition.y;
            }
        }

		Vector2Int gridSize = new Vector2Int(Mathf.RoundToInt((maxXPosition - minXPosition) / nodeDiameter), Mathf.RoundToInt((maxYPosition - minYPosition) / nodeDiameter));

        gridSizeX = gridSize.x;
		gridSizeY = gridSize.y;
		

        grid = new Node[gridSize.x+1, gridSize.y+1];

		foreach(KeyValuePair<Vector2, GridElement> element in generatedNodes)
		{
            Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt((element.Value.FlatPosition.x - minXPosition) / nodeDiameter), Mathf.RoundToInt((element.Value.FlatPosition.y - minYPosition) / nodeDiameter));

			grid[gridPosition.x, gridPosition.y] = new Node(element.Value.Walkable, element.Value, element.Value.WorldPosition, gridPosition.x, gridPosition.y);
        }

        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int y = 0; y <= gridSize.y; y++)
            {
				if (grid[x,y] == null)
				{
                    grid[x, y] = new Node(false, null, new Vector2(x * nodeDiameter - nodeDiameter, y * nodeDiameter), x, y);
                }
            }
        }

		gridDisplayer.OnSetGrid(grid, gridSizeX, gridSizeY);
	}

    /// <summary>
    /// Get the Node at its Grid Position.
    /// </summary>
    /// <param name="x">Position on X on the grid.</param>
    /// <param name="y">Position on Y on the grid.</param>
    /// <returns></returns>
    public Node GetNode(int x, int y)
    {
		if(x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
			return grid[x, y];
		}

		return null;
    }

    /// <summary>
    /// Get a Node from its Worl Position.
    /// </summary>
    /// <param name="worldPosition">The World Position to search for.</param>
    /// <returns>The Node at the World Position wanted.</returns>
    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt((worldPosition.x - minXPosition) / nodeDiameter), Mathf.RoundToInt((worldPosition.y - minYPosition) / nodeDiameter));
       
		if (gridPos.x < 0 || gridPos.x >= gridSizeX || gridPos.y < 0 || gridPos.y >= gridSizeY)
        {
            return null;
        }

        return grid[gridPos.x, gridPos.y];
    }

    /// <summary>
    /// Check if 2 Node are neighbours.
    /// </summary>
    /// <param name="n1"></param>
    /// <param name="n2"></param>
    /// <returns></returns>
    public bool AreNodeNeighbours(Node n1, Node n2)
    {
        return GetNeighbours(n1).Contains(n2);
    }

    /// <summary>
    /// Get every neighbours of the Node.
    /// </summary>
    /// <param name="node">The Node to search around.</param>
    /// <returns>Every Node that are neighbours of the asked Node.</returns>
    public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.GridX + x;
				int checkY = node.GridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) 
				{
                    neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}

	/// <summary>
	/// Get a random neighbour Node of the asked Node.
	/// </summary>
	/// <param name="node">The Node to search around.</param>
	/// <returns>A random Node around the asked Node.</returns>
	public Node GetRandomNeighbours(Node node)
    {
		List<Node> neighbours = GetNeighbours(node);
		return neighbours[Random.Range(0, neighbours.Count)];
    }

    /// <summary>
    /// Get the closest neighbour of a Node.
    /// </summary>
    /// <param name="nodeAsDistanceFrom">The Node to check for distance from.</param>
    /// <param name="nodeToCheckForNeighbours">The Node to search around.</param>
    /// <param name="isDistanceWalkable">Should the distance be walkable.</param>
    /// <returns></returns>
    public Node GetClosestNeighbour(Node nodeAsDistanceFrom, Node nodeToCheckForNeighbours, bool isDistanceWalkable)
	{
        List<Node> neighbours = GetNeighbours(nodeToCheckForNeighbours);

		Node toReturn = null;

		for(int i = 0; i < neighbours.Count; i++)
		{
			if (!isDistanceWalkable || neighbours[i].IsWalkable)
			{
				if (toReturn == null)
				{
					toReturn = neighbours[i];
				}
				else if (Pathfinding.Instance.GetDistance(nodeAsDistanceFrom, neighbours[i]) < Pathfinding.Instance.GetDistance(nodeAsDistanceFrom, toReturn))
				{
					toReturn = neighbours[i];
				}
			}
		}

		return toReturn;
    }

#if UNITY_EDITOR
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));

		if (grid != null && displayGridGizmos) 
		{
            foreach (Node n in grid) {
				bool redColor = false;//n.IsWalkable;
				if(n.LinkedElement != null)
				{
					redColor = n.LinkedElement.Walkable;
				}
				Gizmos.color = redColor?Color.white:Color.red;
				Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter/2));
			}
		}
	}
#endif
}
