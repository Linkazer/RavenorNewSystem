using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SKL_Shape_TargetNode : SKL_SkillActionShape
{
    public override List<Node> GetZone(Node casterNode, Node targetNode)
    {
        return new List<Node>() { targetNode };
    }
}
