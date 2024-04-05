using ReferencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Scriptable", menuName = "Status/New Status")]
public class StatusData : ScriptableObject
{
    [SerializeField] private RVN_Text statusName;
    [SerializeField] private RVN_Text description;
    [SerializeField] private Sprite icon;

    [SerializeField] private Poolable_FX statusFX;

    [SerializeField, SerializeReference, ReferenceEditor(typeof(StatusEffect))] private StatusEffect[] effects;

    public string Name => statusName.GetText();
    public string Description => description.GetText();
    public Sprite Icon => icon;

    public Poolable_FX StatusFX => statusFX;

    public StatusEffect[] Effects => effects;
}
