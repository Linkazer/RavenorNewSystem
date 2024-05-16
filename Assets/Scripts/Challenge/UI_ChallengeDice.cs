using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_ChallengeDice : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private Animator animator;
    [SerializeField] private float rollDuration;

    [SerializeField] private TextMeshProUGUI resultText;

    private Timer rollTimer;

    private int rollStage = 0;
    private bool succeedDice = false;

    public int RollStage => rollStage;

    public void Activate()
    {
        rollTimer?.Stop();

        parentObject.SetActive(true);
        SetRollStage(0);
    }

    public void Deactivate()
    {
        SetRollStage(0);
        parentObject.SetActive(false);
    }

    public void RollDice(int result, bool diceSuccess, float rollDurationOffset)
    {
        SetDiceSuccess(diceSuccess);
        SetRollStage(1);
        resultText.text = result.ToString();
        rollTimer = TimerManager.CreateRealTimer(rollDuration + rollDurationOffset, OnDiceRolled);
    }

    public void OnDiceRolled()
    {
        SetRollStage(2);
    }

    private void SetRollStage(int toSet)
    {
        rollStage = toSet;
        animator.SetInteger("RollStage", rollStage);
    }

    private void SetDiceSuccess(bool toSet)
    {
        succeedDice = toSet;
        animator.SetBool("Success", succeedDice);
    }
}
