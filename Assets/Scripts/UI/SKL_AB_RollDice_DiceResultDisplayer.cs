using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SKL_AB_RollDice_DiceResultDisplayer : MonoBehaviour
{
    [Serializable]
    private struct DisplayableDice
    {
        public GameObject diceHolder;
        public Image resultImage;
        public TextMeshProUGUI resultText;
    }

    [Header("Timer")]
    [SerializeField] private float displayTime;

    [Header("Sprites")]
    [SerializeField] private Sprite successDice;
    [SerializeField] private Sprite failedDice;
    [SerializeField] private Sprite successDiceWithMissedReroll;
    [SerializeField] private Sprite failedDiceWithSuccessReroll;
    [SerializeField] private Sprite successDiceWithDoubleReroll;
    [SerializeField] private Sprite failedDiceWithDoubleReroll;

    [Header("Dices")]
    [SerializeField] private DisplayableDice[] displayableDices;

    public void DisplayDices(List<Dice> dicesToDisplay)
    {
        for (int i = 0; i < displayableDices.Length; i++)
        {
            if(i >= dicesToDisplay.Count)
            {
                displayableDices[i].diceHolder.SetActive(false);
            }
            else
            {
                if (dicesToDisplay[i].DoesHit)
                {
                    if (dicesToDisplay[i].rerolledForOffensive)
                    {
                        if (dicesToDisplay[i].rerolledForDefensive)
                        {
                            displayableDices[i].resultImage.sprite = successDiceWithDoubleReroll;
                        }
                        else
                        {
                            displayableDices[i].resultImage.sprite = successDiceWithMissedReroll;
                        }
                    }
                    else
                    {
                        displayableDices[i].resultImage.sprite = successDice;
                    }

                    displayableDices[i].resultText.text = dicesToDisplay[i].Result.ToString("F0");
                }
                else
                {
                    if (dicesToDisplay[i].rerolledForDefensive)
                    {
                        if (dicesToDisplay[i].rerolledForOffensive)
                        {
                            displayableDices[i].resultImage.sprite = failedDiceWithDoubleReroll;
                        }
                        else
                        {
                            displayableDices[i].resultImage.sprite = failedDiceWithSuccessReroll;
                        }
                    }
                    else
                    {
                        displayableDices[i].resultImage.sprite = failedDice;
                    }

                    displayableDices[i].resultText.text = "";
                }

                displayableDices[i].diceHolder.SetActive(true);
            }
        }

        TimerManager.CreateRealTimer(displayTime, RemoveDisplayer);
    }

    private void RemoveDisplayer()
    {
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
