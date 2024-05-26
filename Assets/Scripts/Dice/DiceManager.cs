using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager
{
    public static Dice RollDice(MonoBehaviour asker, int faceBonus = 0)
    {
        Dice toReturn = new Dice(faceBonus);
        toReturn.Roll(asker);

        return toReturn;
    }

    public static List<Dice> RollDices(MonoBehaviour asker, int diceNumber, int faceBonus = 0)
    {
        List<Dice> toReturn = new List<Dice>();

        for(int i = 0; i < diceNumber; i++)
        {
            toReturn.Add(RollDice(asker, faceBonus));
        }

        return toReturn;
    }
}
