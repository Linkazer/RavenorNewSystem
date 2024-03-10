using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager
{
    public static List<Dice> RollDices(int diceNumber, MonoBehaviour asker, int faceBonus = 0)
    {
        List<Dice> toReturn = new List<Dice>();

        for(int i = 0; i < diceNumber; i++)
        {
            toReturn.Add(new Dice(faceBonus));
            toReturn[i].Roll(asker);
        }

        return toReturn;
    }
}
