using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusTrigger
{
    public abstract void SetTrigger(AppliedStatus appliedStatus);

    public abstract void UnsetTrigger(AppliedStatus appliedStatus);
}
