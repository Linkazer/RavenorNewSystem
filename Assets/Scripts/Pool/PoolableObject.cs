using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    private PoolableObject poolPrefab;

    public PoolableObject PoolPrefab => poolPrefab;

    public virtual void Initialize(PoolableObject basePrefab)
    {
        poolPrefab = basePrefab;
        gameObject.SetActive(true);
    }

    protected virtual void Disable()
    {
        gameObject.SetActive(false);
        PoolManager.DisablePoolableObject(this);
    }
}
