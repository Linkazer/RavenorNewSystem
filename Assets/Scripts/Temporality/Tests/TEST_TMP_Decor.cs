using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_TMP_Decor : MonoBehaviour, IRoundHandler
{
    public void EndRound()
    {
        Debug.Log(gameObject + " End Round");
    }

    public void StartRound()
    {
        Debug.Log(gameObject + " Start Round");
    }

    void Start()
    {
        RoundManager.Instance.AddHandler(this);
    }

    void OnDestroy()
    {
        RoundManager.Instance?.RemoveHandler(this);
    }
}
