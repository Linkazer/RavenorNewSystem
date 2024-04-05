using ReferencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StatusEffect
{
    [SerializeField] protected StatusTrigger trigger;

    public StatusTrigger Trigger => trigger;

    public abstract void DoEffect(AppliedStatus appliedStatus);

    public abstract void UndoEffect(AppliedStatus appliedStatus);
}
