using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChallengeRoll : MonoBehaviour
{
    [SerializeField] private UI_ChallengeDice[] dices;
    [SerializeField] private GameObject secondLine;

    [Header("Tests")]
    public int TEST_DiceAmount;
    public int TEST_ResultNeeded;

    private void OnEnable()
    {
        //Tests
        InitializeChallenge(TEST_DiceAmount);
    }

    public void InitializeChallenge(int diceAmount)
    {
        if(diceAmount > 2)
        {
            secondLine.SetActive(true);
        }
        else
        {
            secondLine.SetActive(false);
        }

        for(int i = 0; i < dices.Length; i++)
        {
            if(i < diceAmount)
            {
                dices[i].Activate();
            }
            else
            {
                dices[i].Deactivate();
            }
        }
    }

    public void UE_RollDice(UI_ChallengeDice diceToRoll, float durationOffset)
    {
        if (diceToRoll.RollStage == 0)
        {
            int result = Random.Range(1, 9);
            diceToRoll.RollDice(result, result > TEST_ResultNeeded, durationOffset);
        }
    }

    public void UE_RollAllDices()
    {
        /*foreach(UI_ChallengeDice dice in dices)
        {
            UE_RollDice(dice);
        }*/

        StartCoroutine(RollAllDices());
    }

    private IEnumerator RollAllDices()
    {
        List<UI_ChallengeDice> diceToRoll = new List<UI_ChallengeDice>();

        foreach (UI_ChallengeDice dice in dices)
        {
            if (dice.RollStage == 0)
            {
                diceToRoll.Add(dice);
            }
        }

        float durationOffset = 0.01f * diceToRoll.Count;

        foreach(UI_ChallengeDice rollingDice in diceToRoll)
        {
            UE_RollDice(rollingDice, durationOffset);

            yield return new WaitForSeconds(0.02f);
            durationOffset -= 0.01f;
        }
    }
}
