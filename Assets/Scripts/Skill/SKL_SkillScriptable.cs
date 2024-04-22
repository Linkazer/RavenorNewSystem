using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum SkillComplexity
{
    Ordinary,
    Fast,
}

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/Create Skill")]
public class SKL_SkillScriptable : ScriptableObject
{
    [Header("Général Informations")]
    [SerializeField] private RVN_Text displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private RVN_Text description;

    [Header("Skill data")]
    [Header("Usability")]
    [SerializeField] private int cooldown;
    [SerializeField] private SkillComplexity castComplexity;
    [SerializeField] private int useByLevel = -1;
    [SerializeField] private int useByTurn;

    [Header("Ressource")]
    [SerializeField] private int ressourceCost;

    [Header("Shape")]
    [SerializeField] private float range;
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_SkillActionShape))] private SKL_SkillActionShape privisualizedShape;

    [Header("Skill Effect")]
    [SerializeField] private SKL_SkillActionChooser skillActions;

    public string DisplayName => displayName.GetText();
    public string Description => description.GetText();
    public Sprite Icon => icon;

    public int RessourceCost => ressourceCost;
    public int MaxUtilisationsByTurn => useByTurn;
    public int MaxUtilisationsByLevel => useByLevel;

    public int Cooldown => cooldown;

    public float Range => range;
    public SkillComplexity CastComplexity => castComplexity;

    public SKL_SkillActionChooser SkillActions => skillActions;

    public SKL_SkillAction GetFirstUsableSkillAction(SKL_ResolvingSkillData resolveData)
    {
        return skillActions.GetFirstUsableAction(resolveData);
    }

    public List<Node> GetDisplayShape(Node casterNode, Node targetNode)
    {
        return privisualizedShape.GetZone(casterNode, targetNode);
    }
}