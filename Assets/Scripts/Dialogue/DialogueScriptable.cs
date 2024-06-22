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
    [SerializeField, SerializeReference, ReferenceEditor(typeof(DialogueAction))] private DialogueAction[] actions; //TODO : Voir si on sort ça (pour l'instant, ça pose pas de problème, mais ça en posera si jamais on a besoin de sauvegarder des données de runtime)

    public DialogueAction[] Actions => actions;
}
