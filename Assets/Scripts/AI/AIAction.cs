using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction
{
    public CharacterEntity character;
    public SKL_SkillScriptable skillToUse;
    public Node movementTarget;
    public Node skillTarget;
    public List<Node> hitedNodes = new List<Node>();
    public float distanceToTravel;
}
