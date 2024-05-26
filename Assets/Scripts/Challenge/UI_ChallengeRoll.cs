using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ChallengeRoll : MonoBehaviour
{
    private struct ResolvingChallenge
    {
        public ChallengeRollData challenge;
        public EC_TraitHandler challengeTarget;
        public List<UI_ChallengeDice> dicesUsedForChallenge;
        public List<UI_ChallengeDice> alreadyRolledDices;

        public Action<int> resultCallback;
    }

    private const int BaseChallengeDiceAmount = 3;

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Expertise")]
    [SerializeField] private TextMeshProUGUI expertiseTitle;

    [Header("Buttons")]
    [SerializeField] private Button diceRollButton;
    [SerializeField] private Button validateResultButton;

    [Header("Dices")]
    [SerializeField] private UI_ChallengeDice[] dices;
    [SerializeField] private GameObject secondLine;

    private ResolvingChallenge currentChallenge;

    public void InitializeChallenge(ChallengeRollData nChallengeRoll, EC_TraitHandler nChallengedHandler, Action<int> callback)
    {
        List<UI_ChallengeDice> usedDices = new List<UI_ChallengeDice>();

        int diceAmount = BaseChallengeDiceAmount + nChallengedHandler.GetExpertiseLevel(nChallengeRoll.expertise);

        if (diceAmount > 2)
        {
            secondLine.SetActive(true);
        }
        else
        {
            secondLine.SetActive(false);
        }

        for (int i = 0; i < dices.Length; i++)
        {
            if (i < diceAmount)
            {
                dices[i].Activate();
                usedDices.Add(dices[i]);
            }
            else
            {
                dices[i].Deactivate();
            }
        }

        currentChallenge = new ResolvingChallenge
        {
            challenge = nChallengeRoll,
            challengeTarget = nChallengedHandler,
            dicesUsedForChallenge = usedDices,
            alreadyRolledDices = new List<UI_ChallengeDice>(),
            resultCallback = callback
        };

        expertiseTitle.text = currentChallenge.challenge.expertise.Title;

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        diceRollButton.gameObject.SetActive(true);

        validateResultButton.gameObject.SetActive(false);
    }

    public void UE_RollDice(UI_ChallengeDice diceToRoll, float durationOffset)
    {
        if (currentChallenge.dicesUsedForChallenge.Contains(diceToRoll) && !currentChallenge.alreadyRolledDices.Contains(diceToRoll))
        {
            int bonus = currentChallenge.challengeTarget.GetExpertiseTraitValue(currentChallenge.challenge.expertise);

            int result = Mathf.FloorToInt(DiceManager.RollDice(currentChallenge.challengeTarget, bonus).Result);

            diceToRoll.RollDice(result, result > currentChallenge.challenge.difficulty, durationOffset);

            currentChallenge.alreadyRolledDices.Add(diceToRoll);

            if(currentChallenge.alreadyRolledDices.Count == currentChallenge.dicesUsedForChallenge.Count)
            {
                OnRolledLastDice();
            }
        }
    }

    private void OnRolledLastDice()
    {
        diceRollButton.gameObject.SetActive(false);

        validateResultButton.gameObject.SetActive(true);
    }

    public void UE_RollAllDices()
    {
        StartCoroutine(RollAllDices());
    }

    public void UE_ValidateChallenge()
    {
        int finalResult = 0;

        foreach (UI_ChallengeDice dice in currentChallenge.dicesUsedForChallenge)
        {
            if (dice.Success)
            {
                finalResult++;
            }
        }

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        currentChallenge.resultCallback?.Invoke(finalResult);
    }

    private IEnumerator RollAllDices()
    {
        List<UI_ChallengeDice> diceToRoll = new List<UI_ChallengeDice>();

        foreach (UI_ChallengeDice dice in currentChallenge.dicesUsedForChallenge)
        {
            if (!currentChallenge.alreadyRolledDices.Contains(dice))
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
