using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StatusEffect
{
    //[SerializeField, ReferenceEditor(typeof(StatusTrigger))] protected StatusTrigger trigger;

    public abstract void ApplyEffect(AppliedStatus appliedStatus);

    public abstract void RemoveEffect(AppliedStatus appliedStatus);
}
