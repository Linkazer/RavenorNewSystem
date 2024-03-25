using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_ResolvingSkillData
{
    private SKL_SkillScriptable skillScriptable;
    private EC_SkillHandler skillCaster;
    private Node targetNode;

    public List<Dice> dicesResult = new List<Dice>();

    public SKL_SkillScriptable SkillData => skillScriptable;
    public EC_SkillHandler Caster => skillCaster;
    public Node TargetNode => targetNode;

    public SKL_ResolvingSkillData(SKL_SkillScriptable nSkillScriptable, EC_SkillHandler nSkillCaster, Node nTargetNode)
    {
        skillScriptable = nSkillScriptable;
        skillCaster = nSkillCaster;
        targetNode = nTargetNode;
    }
}
