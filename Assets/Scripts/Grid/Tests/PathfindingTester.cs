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

    private void Start()
    {
        InputManager.Instance.OnMouseLeftDown += MoveStartPosition;
        InputManager.Instance.OnMouseRightDown += MoveEndPosition;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMouseLeftDown -= MoveStartPosition;
        InputManager.Instance.OnMouseRightDown -= MoveEndPosition;
    }

    private void MoveEndPosition(Vector2 pos)
    {
        endPosition.position = pos;
        SearchForPath();
    }

    private void MoveStartPosition(Vector2 pos)
    {
        startPosition.position = pos;
        SearchForPath();
    }

    [ContextMenu("Search for Path")]
    private void SearchForPath()
    {
        GridZoneDisplayer.Instance.OnUnsetGridFeedback();

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
        GridZoneDisplayer.Instance.OnSetGridFeedback(new List<Node>(pathFound), pathColor);
    }
}
