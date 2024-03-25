using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_Shape_CasterNode : SKL_SkillActionShape
{
    public override List<Node> GetZone(Node casterNode, Node targetNode)
    {
        return new List<Node>() { casterNode };
    }
}
