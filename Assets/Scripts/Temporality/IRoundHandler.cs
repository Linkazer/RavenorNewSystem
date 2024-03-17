using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoundHandler
{
    /// <summary>
    /// Called at the start of a Round.
    /// </summary>
    public void StartRound();

    /// <summary>
    /// Called at the end of a Round.
    /// </summary>
    public void EndRound();
}
