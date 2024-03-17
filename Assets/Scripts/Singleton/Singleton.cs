using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance = null;

    public static T Instance => instance;

    public static Action WaitForInitialization;

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this);

            return;
        }
        else
        {
            instance = this as T;
            WaitForInitialization?.Invoke();
            WaitForInitialization = null;
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
