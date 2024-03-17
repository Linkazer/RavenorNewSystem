using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_TMP_Chara : MonoBehaviour, IRoundHandler
{
    public TEST_TMP_Effect effects = null;

    public void EndRound()
    {
        Debug.Log(gameObject + " End Round");
        effects?.Progress();
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
