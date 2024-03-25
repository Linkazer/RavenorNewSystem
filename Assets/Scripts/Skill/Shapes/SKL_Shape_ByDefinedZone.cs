using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_Shape_ByDefinedZone : SKL_SkillActionShape
{
    [SerializeField] private List<Vector2Int> zoneDefined;
    [SerializeField] private bool zoneFaceCaster;

    public override List<Node> GetZone(Node casterNode, Node targetNode)
    {
        List<Node> toReturn = new List<Node>();

        Vector2 direction = Vector2.one;
        if (zoneFaceCaster && casterNode != null)
        {
            direction = new Vector2(targetNode.GridX, targetNode.GridY) - new Vector2(casterNode.GridX, casterNode.GridY);
        }

        foreach (Vector2Int v in zoneDefined)
        {
            Node toAdd = null;

            if (direction.y == 0 && direction.x == 0)
            {
                toAdd = Grid.Instance.GetNode(targetNode.GridX + v.x, targetNode.GridY + v.y);
            }
            else if (direction.y > 0 && (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) || direction.x == direction.y))
            {
                toAdd = Grid.Instance.GetNode(targetNode.GridX + v.x, targetNode.GridY + v.y);
            }
            else if (direction.x < 0 && (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) || direction.x == -direction.y))
            {
                toAdd = Grid.Instance.GetNode(targetNode.GridX - v.y, targetNode.GridY + v.x);
            }
            else if (direction.y < 0 && (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) || direction.x == direction.y))
            {
                toAdd = Grid.Instance.GetNode(targetNode.GridX - v.x, targetNode.GridY - v.y);
            }
            else
            {
                toAdd = Grid.Instance.GetNode(targetNode.GridX + v.y, targetNode.GridY - v.x);
            }

            if (toAdd != null)
            {
                toReturn.Add(toAdd);
            }
        }

        return toReturn;
    }
}
