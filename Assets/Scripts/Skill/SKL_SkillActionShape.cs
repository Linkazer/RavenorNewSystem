using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SKL_SkillActionShape
{
    public abstract List<Node> GetZone(Node casterNode, Node targetNode);
}
