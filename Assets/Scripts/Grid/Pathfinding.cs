using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : Singleton<Pathfinding> 
{
	[SerializeField] private Grid grid;
	
	private void OnStartFindPath(Vector3 startPos, Vector3 targetPos, int walkDistance) {
		FindPath(startPos,targetPos, walkDistance);
	}
	
	private void FindPath(Vector3 startPos, Vector3 targetPos, int walkDistance) {

		Node[] waypoints = new Node[0];
		bool pathSuccess = true;
		
		Node startNode = grid.GetNodeFromWorldPoint(startPos);
		Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

		waypoints = CalculatePathfinding(startNode, targetNode, walkDistance).ToArray();

		if (waypoints.Length <= 0 || waypoints[0] == null)
        {
			pathSuccess = false;
        }

        //TODO : Return le succès du Path et passer le callback
        //requestManager.FinishedProcessingPath(waypoints,pathSuccess);
    }

    private Node[] RetracePath(Node startNode, Node endNode, int maxDistance) {

		Node[] path = GetPath(startNode, endNode, maxDistance).ToArray();

		Array.Reverse(path);
		return path;
	}
	
	private Vector3[] GetVectorPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		
		for (int i = 0; i < path.Count; i ++) {
				waypoints.Add(path[i].WorldPosition);
		}
		return waypoints.ToArray();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="startNode"></param>
	/// <param name="targetNode">If null, will set all ode in the distance.</param>
	/// <param name="isForNextTurn"></param>
	/// <returns></returns>
	public bool SearchPath(Node startNode, Node targetNode)
    {
		List<Node> walkableNodes = CalculatePathfinding(startNode, targetNode, -1);
		return walkableNodes.Count > 0;
	}

	private List<Node> GetPath(Node startNode, Node endNode, int maxDistance)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		List<Node> checkedNodes = new List<Node>();

		while (maxDistance >= 0 && currentNode.gCost > maxDistance && !checkedNodes.Contains(currentNode))
        {
			checkedNodes.Add(currentNode);
			if (currentNode.parent != null)
			{
				currentNode = currentNode.parent;
			}
			else
            {
				break;
            }
		}

		while (currentNode != startNode)
		{
			if (currentNode.parent == null)
            {
				path = new List<Node>();
				break;
			}
			else
			{
				path.Add(currentNode);
			}
			currentNode = currentNode.parent;
		}

		return path;
	}
	
	public int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
		int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);
		
		if (dstX > dstY)
			return 15*dstY + 10* (dstX-dstY);
		return 15*dstX + 10 * (dstY-dstX);
	}

	public int GetPathLength(Node startNode, List<Node> path)
	{
		int distance = GetDistance(startNode, path[0]);

		for(int i = 0; i < path.Count - 2; i++)
		{
			distance += GetDistance(path[i], path[i + 1]);
		}

		return distance;
	}

	private bool IsNodeUsable(Node nodeToCheck, Node currentNode, Node targetNode,  bool checkNonStaticObstacle, bool targetIsObstacle)
	{
        if (nodeToCheck == targetNode)
        {
            return true;
        }

		if (!CanDiagonalBeReached(currentNode, nodeToCheck))
		{
			return false;
		}
        
        if (targetIsObstacle && targetNode != null)
		{
			
			if(checkNonStaticObstacle)
			{
                return nodeToCheck.IsWalkable;
            }
			else
			{
                return !nodeToCheck.IsStaticObstacle;
            }
		}
		else
		{
            if (checkNonStaticObstacle)
            {
                return nodeToCheck.IsWalkable;
            }
            else
            {
                return !nodeToCheck.IsStaticObstacle;
            }
        }
    }

    /// <summary>
    /// Calcul of the pathfinding.
    /// </summary>
    /// <param name="startNode">The node where the calcul start.</param>
    /// <param name="targetNode">The target wanted. If null, the method will return all the node in the distance.</param>
    /// <param name="distance">The max distance to check for node. Is only check if more than 0.</param>
    /// <param name="pathCalcul"></param>
    /// <returns></returns>
    public List<Node> CalculatePathfinding(Node startNode, Node targetNode, int distance, bool checkNonStaticObstacle = true, bool targetIsObstacle = false)
	{
		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		List<Node> usableNode = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();

		startNode.gCost = 0;

		openSet.Add(startNode);
		usableNode.Add(startNode);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			foreach (Node neighbour in grid.GetNeighbours(currentNode))
			{ 
                if (closedSet.Contains(neighbour) || !IsNodeUsable(neighbour, currentNode, targetNode, checkNonStaticObstacle, targetIsObstacle))
				{
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (distance <= 0 || newMovementCostToNeighbour <= distance)
				{
                    int newDistanceFromTargetCost = -1;

					if (newMovementCostToNeighbour <= neighbour.gCost || !openSet.Contains(neighbour))
					{
                        neighbour.gCost = newMovementCostToNeighbour;
						if (newDistanceFromTargetCost < 0)
						{
							neighbour.hCost = 0;
						}
						else
						{
							neighbour.hCost = newDistanceFromTargetCost;
						}
						neighbour.parent = currentNode;
						currentNode.children = neighbour;

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
							if (!usableNode.Contains(neighbour))
							{
								usableNode.Add(neighbour);
							}
						}
					}

					if (targetNode == neighbour)
					{
						if (targetIsObstacle || targetNode.IsWalkable)
						{
							return new List<Node>(RetracePath(startNode, targetNode, distance));
						}
						else
						{
                            return new List<Node>(RetracePath(startNode, currentNode, distance));
                        }
					}
				}
			}
		}

		if(targetNode != null)
        {
			usableNode = new List<Node>();
        }

        return usableNode;
	}

	public List<Node> GetAllNodeInDistance(Node startNode, int distance, bool needVision)
    {
		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		List<Node> usableNode = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();

		startNode.gCost = 0;

		openSet.Add(startNode);
		usableNode.Add(startNode);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);
			foreach (Node neighbour in grid.GetNeighbours(currentNode))
			{
				if (closedSet.Contains(neighbour))
				{
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

				if (newMovementCostToNeighbour <= distance && (!needVision || IsNodeVisible(startNode, neighbour)))
				{
					if (newMovementCostToNeighbour <= neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = 0;
						neighbour.parent = currentNode;
						currentNode.children = neighbour;

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
							if (!usableNode.Contains(neighbour))
							{
								usableNode.Add(neighbour);
							}
						}
					}
				}
			}
		}

		return usableNode;
	}

	/// <summary>
	/// Utilisé pour vérifier si 2 Nodes en diagonales peuvent être utilisées.
	/// </summary>
	/// <param name="startNode"></param>
	/// <param name="targetNode"></param>
	/// <returns></returns>
	private bool CanDiagonalBeReached(Node startNode, Node targetNode)
    {
		Vector2Int direction = new Vector2Int(targetNode.GridX - startNode.GridX, targetNode.GridY - startNode.GridY);

		if(direction.x == 0 || direction.y == 0)
        {
			return true;
        }

		bool xAxis = grid.GetNode(startNode.GridX + direction.x, startNode.GridY).IsWalkable;
		bool yAxis = grid.GetNode(startNode.GridX, startNode.GridY + direction.y).IsWalkable;

		return xAxis || yAxis;
    }


    public bool IsNodeVisible(Node startNode, Node targetNode, float distanceMax = -1)
    {
        if (distanceMax > 0 && GetDistance(startNode, targetNode) > distanceMax)
        {
            return false;
        }
        else
        {
            return CalculateVision(startNode, targetNode);
        }
    }

    private bool CalculateVision(Node startNode, Node visibilityTargetNode)
    {
        int dx = visibilityTargetNode.GridX - startNode.GridX;
        int dy = visibilityTargetNode.GridY - startNode.GridY;
        int nx = Mathf.Abs(dx);
        int ny = Mathf.Abs(dy);
        int signX = dx > 0 ? 1 : -1;
        int signY = dy > 0 ? 1 : -1;

        Vector2Int p = new Vector2Int(startNode.GridX, startNode.GridY);

        for (int ix = 0, iy = 0; ix < nx || iy < ny;)
        {
            int decision = (1 + 2 * ix) * ny - (1 + 2 * iy) * nx;

            if (decision == 0)
            {
                //Diagonal
                if (grid.GetNode(p.x, p.y + signY).IsVisible || grid.GetNode(p.x + signX, p.y).IsVisible)
                {
                    p.x += signX;
                    ix++;
                    p.y += signY;
                    iy++;
                }
                else
                {
                    return false;
                }
            }
            else if (decision < 0)
            {
                //Horizontal
                p.x += signX;
                ix++;
            }
            else
            {
                //Vertical
                p.y += signY;
                iy++;
            }

            if (!grid.GetNode(p.x, p.y).IsVisible && (grid.GetNode(p.x, p.y) != visibilityTargetNode || visibilityTargetNode.IsStaticObstacle))
            {
                return false;
            }
        }

        return true;
    }
}
