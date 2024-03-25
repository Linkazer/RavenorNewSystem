using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : Singleton<Pathfinding> 
{
	public const float DirectDistance = 1.5f;
	private const float DiagonaleDistance = 2.25f;

	[SerializeField] private Grid grid;
	
	/// <summary>
	/// Try to find a Path between 2 Nodes.
	/// </summary>
	/// <param name="startPos">The starting Node.</param>
	/// <param name="targetPos">The target Node.</param>
	/// <param name="pathMaxDistance">The maximum distance of the Path.</param>
	/// <param name="onPathFound">Callback to use if a Path is found.</param>
	/// <param name="checkNonStaticObstacle">Does the path Check or Ignore NonStatic obstacle.</param>
	/// <param name="targetIsObstacle">Is the target an obstacle.</param>
	/// <returns>TRUE if a Path is found.</returns>
	public bool TryFindPath(Vector3 startPos, Vector3 targetPos, float pathMaxDistance, Action<Node[]> onPathFound, bool checkNonStaticObstacle = true, bool targetIsObstacle = false)
	{
        Node[] path = new Node[0];
        bool hasFoundPath = true;

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        path = CalculatePathfinding(startNode, targetNode, pathMaxDistance, checkNonStaticObstacle, targetIsObstacle).ToArray();

        if (path.Length <= 0 || path[0] == null)
        {
            hasFoundPath = false;
        }

		if(hasFoundPath)
		{
			onPathFound?.Invoke(path);
		}

		return hasFoundPath;

        //TODO : Return le succès du Path et passer le callback
        //requestManager.FinishedProcessingPath(waypoints,pathSuccess);
    }

    /// <summary>
    /// Calcul of the pathfinding.
    /// </summary>
    /// <param name="startNode">The node where the calcul start.</param>
    /// <param name="targetNode">The target wanted. If null, the method will return all the node in the distance.</param>
    /// <param name="distance">The max distance to check for node. Is only check if more than 0.</param>
    /// <param name="pathCalcul"></param>
    /// <returns></returns>
    public List<Node> CalculatePathfinding(Node startNode, Node targetNode, float distance, bool checkNonStaticObstacle = true, bool targetIsObstacle = false)
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

                float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

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

        if (targetNode != null)
        {
            usableNode = new List<Node>();
        }

        return usableNode;
    }

    /// <summary>
    /// Check if a Node is usable for the Path calculation.
    /// </summary>
    /// <param name="nodeToCheck">The Node to check.</param>
    /// <param name="currentNode">The current Node in the path calculation.</param>
    /// <param name="targetNode">The target Node of the path.</param>
    /// <param name="checkNonStaticObstacle">Does the path Check or Ignore NonStatic obstacle.</param>
    /// <param name="targetIsObstacle">Is the target an obstacle.</param>
    /// <returns>TRUE if the Node can be used for the Path calculation.</returns>
    private bool IsNodeUsable(Node nodeToCheck, Node currentNode, Node targetNode, bool checkNonStaticObstacle, bool targetIsObstacle)
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

            if (checkNonStaticObstacle)
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
    /// Retrace the full path.
    /// </summary>
    /// <param name="startNode">The starting Node.</param>
    /// <param name="endNode">The target Node.</param>
    /// <param name="maxDistance">The maximum distance of the Path.</param>
    /// <returns></returns>
    private Node[] RetracePath(Node startNode, Node endNode, float maxDistance) {

		Node[] path = GetPath(startNode, endNode, maxDistance).ToArray();

		Array.Reverse(path);
		return path;
	}

    /// <summary>
    /// Retrace every parent node until the path is completed.
    /// </summary>
    /// <param name="startNode">The starting Node.</param>
    /// <param name="endNode">The target Node.</param>
    /// <param name="maxDistance">The maximum distance of the Path.</param>
    /// <returns></returns>
    private List<Node> GetPath(Node startNode, Node endNode, float maxDistance)
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

    /// <summary>
    /// Utilisé pour vérifier si 2 Nodes en diagonales peuvent être utilisées.
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    private bool CanDiagonalBeReached(Node startNode, Node targetNode)
    {
        Vector2Int direction = new Vector2Int(targetNode.GridX - startNode.GridX, targetNode.GridY - startNode.GridY);

        if (direction.x == 0 || direction.y == 0)
        {
            return true;
        }

        bool xAxis = grid.GetNode(startNode.GridX + direction.x, startNode.GridY).IsWalkable;
        bool yAxis = grid.GetNode(startNode.GridX, startNode.GridY + direction.y).IsWalkable;

        return xAxis || yAxis;
    }

	/// <summary>
	/// Get the raw distance between 2 Nodes.
	/// </summary>
	/// <param name="nodeA"></param>
	/// <param name="nodeB"></param>
	/// <returns></returns>
    public float GetDistance(Node nodeA, Node nodeB)
    {
        float dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        float dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return DiagonaleDistance * dstY + DirectDistance * (dstX - dstY);
        return DiagonaleDistance * dstX + DirectDistance * (dstY - dstX);
    }

	/// <summary>
	/// Get the length of a path.
	/// </summary>
	/// <param name="startNode">The start point of the path.</param>
	/// <param name="path">The path to get length from.</param>
	/// <returns></returns>
    public float GetPathLength(Node startNode, List<Node> path)
    {
        float distance = GetDistance(startNode, path[0]);

        for (int i = 0; i < path.Count - 2; i++)
        {
            distance += GetDistance(path[i], path[i + 1]);
        }

        return distance;
    }

	/// <summary>
	/// Check if the TargetNode is visible from the StartNode.
	/// </summary>
	/// <param name="startNode">The Node from where we want to see.</param>
	/// <param name="targetNode">The Node we want to check.</param>
	/// <param name="distanceMax">The maximum distance at which the StartNode can see.</param>
	/// <returns></returns>
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

	/// <summary>
	/// Calculate the vision between 2 Nodes.
	/// </summary>
	/// <param name="startNode"></param>
	/// <param name="visibilityTargetNode"></param>
	/// <returns></returns>
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

    /// <summary>
    /// Get every Node in a distance.
    /// </summary>
    /// <param name="startNode">The Node to search around.</param>
    /// <param name="distance">The distance at which to check.</param>
    /// <param name="needVision">Does the StartNode need vision on the Node.</param>
    /// <returns></returns>
    public List<Node> GetAllNodeInDistance(Node startNode, float distance, bool needVision)
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

                float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

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
}
