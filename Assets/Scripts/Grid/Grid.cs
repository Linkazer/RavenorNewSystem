using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Grid : Singleton<Grid> 
{
	[SerializeField] private bool displayGridGizmos;
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;
	private Node[,] grid;

	private float nodeDiameter;
	private int gridSizeX, gridSizeY;

    [SerializeField] private GridZoneDisplayer gridDisplayer;

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
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

	/// <summary>
	/// Create the Grid.
	/// </summary>
	public void CreateGrid() {
		if (grid == null)
		{
			grid = new Node[gridSizeX, gridSizeY];
		}
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeRadius*0.2f,unwalkableMask));

				if (grid[x, y] != null)
				{
					grid[x, y].SetNode(walkable, worldPoint, x, y);
				}
				else
                {
					grid[x, y] = new Node(walkable, worldPoint, x, y); ;
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
        float percentX = (worldPosition.x + nodeRadius + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + nodeRadius + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX) * percentX) - 1;
        int y = Mathf.RoundToInt((gridSizeY) * percentY) - 1;

        if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY)
        {
            return null;
        }

        return grid[x, y];
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

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
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
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				bool redColor = n.IsWalkable;
				Gizmos.color = redColor?Color.white:Color.red;
				Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter/2));
			}
		}
	}
#endif
}
