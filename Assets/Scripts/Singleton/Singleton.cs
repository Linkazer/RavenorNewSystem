using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance = null;

    public static T Instance => instance;

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
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}

