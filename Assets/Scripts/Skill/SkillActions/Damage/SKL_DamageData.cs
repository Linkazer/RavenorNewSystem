using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SKL_DamageData
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SKL_DamageOrigin))] private SKL_DamageOrigin origin;
    [SerializeField] private SKL_DamageType damageType;

    public SKL_DamageOrigin Origin => origin;
    public SKL_DamageType DamageType => damageType;
}
