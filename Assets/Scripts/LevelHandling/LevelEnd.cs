using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the logic for a type of Level End.
/// </summary>
public abstract class LevelEnd
{
    private LevelHandler levelHandler;

    public void Initialize(LevelHandler handler)
    {
        levelHandler = handler;

        OnInitialize();
    }

    protected abstract void OnInitialize();
}
