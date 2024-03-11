using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTester : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool checkNonStatic = true;
    [SerializeField] private Color pathColor = Color.green;

    [ContextMenu("Search for Path")]
    private void SearchForPath()
    {
        RVN_GridDisplayer.Instance.OnUnsetGridFeedback();

        Debug.Log("Start Searching");

        bool isTargetObstacle = !Grid.Instance.GetNodeFromWorldPoint(endPosition.position).IsWalkable;

        if(Pathfinding.Instance.TryFindPath(startPosition.position, endPosition.position, maxDistance, OnPathFound, checkNonStatic, isTargetObstacle))
        {
            Debug.Log("Path Found");
        }
        else
        {
            Debug.Log("No Path Found");
        }
    }

    private void OnPathFound(Node[] pathFound)
    {
        RVN_GridDisplayer.Instance.OnSetGridFeedback(new List<Node>(pathFound), pathColor);
    }
}
