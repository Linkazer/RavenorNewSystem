using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AIMovementBehavior
{
    protected enum PathType //State of the current calculated path
    {
        PathBlockByMovingObstacle,
        PathClear,
        TooFar,
    }

    public abstract List<Node> GetBestMovementNodes(EC_Movement characterMovement);
}
