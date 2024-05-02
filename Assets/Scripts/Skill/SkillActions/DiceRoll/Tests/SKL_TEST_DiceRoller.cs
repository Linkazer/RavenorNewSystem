using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKL_TEST_DiceRoller : MonoBehaviour
{
    [Header("Test")]
    [SerializeField] private int testsAmount;

    [Header("Dices")]
    [SerializeField] private int numberDicesToRoll;

    [Header("Attacker")]
    [SerializeField] private int traitScore;
    [SerializeField] private int offensiveAdvantage;
    [SerializeField] private int damageBonus;

    [Header("Defender")]
    [SerializeField] private int defenseScore;
    [SerializeField] private int defensiveAdvantage;

    [ContextMenu("Roll Test Dice")]
    private void LaunchTest()
    {
        SortedDictionary<int, int> touchesAmount = new SortedDictionary<int, int>();

        for(int i = 0; i < testsAmount; i++)
        {
            List<Dice> dices = RollDices(numberDicesToRoll, traitScore, defenseScore);

            int hitAmount = 0;
            foreach(Dice d in dices)
            {
                if(d.DoesHit)
                {
                    hitAmount++;
                }
            }

            if(!touchesAmount.ContainsKey(hitAmount))
            {
                touchesAmount.Add(hitAmount, 0);
            }
            touchesAmount[hitAmount]++;
        }

        DisplayResults(touchesAmount);
    }

    private void DisplayResults(SortedDictionary<int, int> touchesAmount)
    {
        string toDisplay = "";

        foreach (KeyValuePair<int, int> pair in touchesAmount)
        {
            toDisplay += ($"{pair.Key} : {((float)pair.Value / (float)testsAmount * 100f).ToString("F2")}% ({pair.Value}) \n");
        }

        Debug.Log(toDisplay);
    }

    private List<Dice> RollDices(int diceNumber, int touchBonus, int defense)
    {
        List<Dice> rolledDices = DiceManager.RollDices(this, diceNumber, touchBonus);

        RollDices(rolledDices, defense);

        return rolledDices;
    }

    private void RollDices(List<Dice> dicesToRoll, int defense)
    {
        float totalHits = 0;
        int currentOffensiveRerolls = 0;
        int currentDefensiveRerolls = 0;

        int maxOffensiveRerolls = offensiveAdvantage;
        int maxDefensiveRerolls = defensiveAdvantage;

        if(maxOffensiveRerolls > maxDefensiveRerolls)
        {
            maxOffensiveRerolls -= maxDefensiveRerolls;
            maxDefensiveRerolls = 0;
        }
        else if(maxDefensiveRerolls > maxOffensiveRerolls)
        {
            maxDefensiveRerolls -= maxOffensiveRerolls;
            maxOffensiveRerolls = 0;
        }
        else
        {
            maxOffensiveRerolls = 0;
            maxDefensiveRerolls = 0;
        }

        for (int i = 0; i < dicesToRoll.Count; i++)
        {
            totalHits += CheckDiceHit(dicesToRoll[i], defense, currentOffensiveRerolls < maxOffensiveRerolls, currentDefensiveRerolls < maxDefensiveRerolls, out bool usedOffensiveReroll, out bool usedDefensiveReroll);

            if (usedDefensiveReroll)
            {
                currentDefensiveRerolls++;
                dicesToRoll[i].rerolledForDefensive = true;
            }

            if (usedOffensiveReroll)
            {
                currentOffensiveRerolls++;
                dicesToRoll[i].rerolledForOffensive = true;
            }
        }
    }

    private int CheckDiceHit(Dice dice, int defense, bool hasOffensiveReroll, bool hasDefensiveReroll, out bool usedOffensiveReroll, out bool usedDefensiveReroll)
    {
        int toReturn = 0;

        usedDefensiveReroll = false;
        usedOffensiveReroll = false;

        if (dice.Result > defense)
        {
            if (hasDefensiveReroll)
            {
                usedDefensiveReroll = true;
                dice.Roll(this);
                return CheckDiceHit(dice, defense, hasOffensiveReroll, false, out usedOffensiveReroll, out bool def);
            }
            else
            {
                dice.DoesHit = true;
                toReturn = 1;
            }
        }
        else if (hasOffensiveReroll)
        {
            usedOffensiveReroll = true;
            dice.Roll(this);
            return CheckDiceHit(dice, defense, false, hasDefensiveReroll, out bool off, out usedDefensiveReroll);
        }

        return toReturn;
    }
}
