using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Scriptable", menuName = "Character/New Character")]
public class CharacterScriptable : DialogueSpeaker, 
    IEC_GridEntityData, 
    IEC_RendererData, 
    IEC_MovementData,
    IEC_HealthHandlerData,
    IEC_TraitHandlerData,
    IEC_SkillHandlerData,
    IEC_SkillAbsorberHandlerData,
    IEC_StatusHandlerData
{
    [SerializeField] private RVN_Text characterTitle;
    [SerializeField] private RVN_Text characterDescription;

    [Header("Movement")]
    [SerializeField] private bool walkable = false;
    [SerializeField] private bool blockVision = true;

    [Header("Render")]
    [SerializeField] private float height = 1f;
    [SerializeField] private AnimationHandler renderer;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float movementByTurn = 25f;

    [Header("Stats")]
    [SerializeField] private int healthPoints;
    [SerializeField] private int armor;
    [SerializeField] private int force;
    [SerializeField] private int esprit;
    [SerializeField] private int presence;
    [SerializeField] private int agilite;
    [SerializeField] private int instinct;

    [Header("Passives")]
    [SerializeField] private StatusData[] passives;

    [Header("Skills")]
    [SerializeField] private List<SKL_SkillScriptable> skills;

    [Header("Ressource")]
    [SerializeField] private SkillRessourceType ressourceUsed;

    [Header("Expertises")]
    [SerializeField] private List<ChallengeExpertiseLevel> expertises;

    public string CharacterTitle => characterTitle.GetText();
    public string CharacterDescription => characterDescription.GetText();

    public bool Walkable => walkable;

    public bool BlockVision => blockVision;

    public AnimationHandler AnimationHandler => renderer;

    public float Speed => speed;

    public float DistanceByTurn => movementByTurn;

    public List<SKL_SkillScriptable> Skills => skills;

    public SkillRessourceType RessourceTypeUsed => ressourceUsed;

    public int MaxHealth => healthPoints;

    public int CurrentHealth => healthPoints;//Utile ? => Peut �tre utile pour les Ennemis (Au lieu d'en faire des Character avec des Stats de base, on les cr�er de 0 avec les stats de combat directement)

    public int MaxArmor => armor;

    public int CurrentArmor => armor;//Utile ?

    public int DodgeBonus => 0;

    public int WillBonus => 0;

    public int DefensiveAdvantage => 0;//Utile ?

    public int DefensiveDisavantage => 0;//Utile ?

    public int Force => force;
    public int Esprit => esprit;
    public int Presence => presence;
    public int Agilite => agilite;
    public int Instinct => instinct;

    public List<ChallengeExpertiseLevel> Expertises => expertises;

    public int OffensiveAdvantage => 0;//Utile ?

    public int OffensiveDisavantage => 0;//Utile ?

    public StatusData[] Passives => passives;

    public float UiHeight => height;
}
