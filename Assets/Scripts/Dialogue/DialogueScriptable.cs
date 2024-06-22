using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The data of a Dialogue.
/// </summary>
[CreateAssetMenu(fileName ="Dialogue Scriptable", menuName ="Dialogue/New Dialogue")]
public class DialogueScriptable : ScriptableObject
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(DialogueAction))] private DialogueAction[] actions; //TODO : Voir si on sort �a (pour l'instant, �a pose pas de probl�me, mais �a en posera si jamais on a besoin de sauvegarder des donn�es de runtime)

    public DialogueAction[] Actions => actions;
}
