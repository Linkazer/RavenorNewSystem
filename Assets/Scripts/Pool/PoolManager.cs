using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<PrefabAssetType, List<Poolable_FX>> instantiatedFX = new Dictionary<PrefabAssetType, List<Poolable_FX>>(); //Peut être utilisé comme base pour un système de Pool complet

    /// <summary>
    /// Instatiate an Animation object at the wanted position.
    /// </summary>
    /// <param name="toPlay">The animation object to instantiate.</param>
    /// <param name="position">The position where the animation is played.</param>
    /// <param name="callback">The callback to call at the end of the animation.</param>
    public static Poolable_FX InstatiatePoolableAtPosition(Poolable_FX toPlay, Vector2 position, Action callback = null)
    {
        Poolable_FX runtimePlayedAnimation = Instantiate(toPlay, position, Quaternion.identity);

        runtimePlayedAnimation.Play(callback);

        return runtimePlayedAnimation;
    }
}
