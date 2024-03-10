using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    private const float coefOnUse = 0.25f;

    public static Dictionary<MonoBehaviour, List<int>> dicesHistory = new Dictionary<MonoBehaviour, List<int>>();

    private int resultBonus;

    private float result;

    public float Result => result;

    public Dice()
    {

    }

    public Dice(int bonus = 0)
    {
        resultBonus = bonus;
    }

    public float Roll(MonoBehaviour asker)
    {
        int diceResult = 0;

        List<int> history = new List<int>();
        if (asker != null)
        {
            if (dicesHistory.ContainsKey(asker))
            {
                history = dicesHistory[asker];
            }
            else
            {
                dicesHistory[asker] = new List<int>();
            }
        }

        List<float> weights = new List<float>() { 1f, 1f, 1f, 1f, 1f, 1f };

        foreach(int hist in history)
        {
            if(hist > 0)
            {
                weights[hist - 1] *= coefOnUse;
            }
        }

        float maxWeight = 0;

        foreach(float f in weights)
        {
            maxWeight += f;
        }

        float rng = UnityEngine.Random.Range(0, maxWeight);

        float currentCount = 0;
        int index = 0;

        foreach (float f in weights)
        {
            currentCount += f;
            index++;
            if (rng < currentCount)
            {
                diceResult = index;
                break;
            }
        }

        try
        {
            if (asker != null)
            {
                dicesHistory[asker].Add((int)diceResult);
            }
        }
        catch(Exception e)
        {
            Debug.Log((int)diceResult);
        }

        if (asker != null)
        {
            if (dicesHistory[asker].Count > 10)
            {
                dicesHistory[asker].RemoveAt(0);
            }
        }

        result = diceResult + resultBonus;

        return diceResult;
    }
}
