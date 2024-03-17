using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Dialogue Scriptable", menuName ="Dialogue/New Dialogue")]
public class DialogueScriptable : ScriptableObject
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(DialogueAction))] private DialogueAction[] actions;

    public DialogueAction[] Actions => actions;
}
