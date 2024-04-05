using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActionHandler : MonoBehaviour
{
    protected PlayerActionManager actionHandler;

    protected bool isLocked;

    public virtual void SetHandler(PlayerActionManager handler)
    {
        actionHandler = handler;
    }

    /// <summary>
    /// Active l'ActionHandler (+ UI).
    /// </summary>
    public abstract void Enable();

    /// <summary>
    /// Désactive l'ActionHandler (+ UI).
    /// </summary>
    public abstract void Disable();

    /// <summary>
    /// Bloque l'ActionHandler (+ UI).
    /// </summary>
    /// <param name="doesLock">Should be locked ?</param>
    public virtual void Lock(bool doesLock)
    {
        if (doesLock != isLocked)
        {
            isLocked = doesLock;
        }
    }
}
