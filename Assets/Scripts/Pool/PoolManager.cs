using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    /// <summary>
    /// Instatiate an Animation object at the wanted position.
    /// </summary>
    /// <param name="toPlay">The animation object to instantiate.</param>
    /// <param name="position">The position where the animation is played.</param>
    /// <param name="callback">The callback to call at the end of the animation.</param>
    public static void InstatiatePoolableAtPosition(Poolable_FX toPlay, Vector2 position, Action callback = null)
    {
        Poolable_FX runtimePlayedAnimation = Instantiate(toPlay, position, Quaternion.identity);

        runtimePlayedAnimation.Play(callback);
    }
}
