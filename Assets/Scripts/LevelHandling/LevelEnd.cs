using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
