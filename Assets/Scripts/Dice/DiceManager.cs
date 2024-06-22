using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager
{
    /// <summary>
    /// Roll a single dice.
    /// </summary>
    /// <param name="asker">The MonoBehavior rolling the dice.</param>
    /// <param name="faceBonus">The bonus of the dice.</param>
    /// <returns>The Dice rolled.</returns>
    public static Dice RollDice(MonoBehaviour asker, int faceBonus = 0)
    {
        Dice toReturn = new Dice(faceBonus);
        toReturn.Roll(asker);

        return toReturn;
    }

    /// <summary>
    /// Roll multiple dices.
    /// </summary>
    /// <param name="asker">The MonoBehavior rolling the dice.</param>
    /// <param name="diceNumber">The number of dices to roll.</param>
    /// <param name="faceBonus">The bonus of the dice.</param>
    /// <returns>A list of all rolled dices.</returns>
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
