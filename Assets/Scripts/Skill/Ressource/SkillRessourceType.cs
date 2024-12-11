using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReferencePicker;

[CreateAssetMenu(fileName ="Ressource Type", menuName ="Skill/Ressource Type")]
public class SkillRessourceType : ScriptableObject
{
    [SerializeField] private RVN_Text title;

    [Header("Ressource Behavior")]
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SkillRessource))] private SkillRessource ressourceBehavior;

    [Header("UI")]
    [SerializeField] private Sprite uiIcon;
    [SerializeField] private Sprite uiPortraitBackground;
    [SerializeField] private Sprite uiSkillBackground;

    public string Title => title.GetText();
    public Sprite UiIcon => uiIcon;
    /// <summary>
    /// Utilisé sur l'affichage de Ressource actuelle du personnage (A côté de son portrait)
    /// </summary>
    public Sprite UiPortraitBackground => uiPortraitBackground;
    /// <summary>
    /// Utilisé sur l'affichage du cout de Ressource d'un skill
    /// </summary>
    public Sprite UiSkillBackground => uiSkillBackground;

    public SkillRessource RessourceBehavior => ressourceBehavior;
}
