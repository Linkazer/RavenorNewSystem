using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_Shape_ByDistance : SKL_SkillActionShape
{
    [SerializeField] private float zoneDistance;
    [SerializeField] private bool needVision = false;

    public override List<Node> GetZone(Node casterNode, Node targetNode)
    {
        return Pathfinding.Instance.GetAllNodeInDistance(casterNode, zoneDistance, needVision);
    }
}
