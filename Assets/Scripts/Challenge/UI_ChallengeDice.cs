using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// The UI of a single Dice for the Challenge Roll.
/// </summary>
public class UI_ChallengeDice : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private Animator animator;
    [SerializeField] private float rollDuration;

    [SerializeField] private TextMeshProUGUI resultText;

    private Timer rollTimer;

    private int rollStage = 0;
    private bool succeedDice = false;

    public bool Success => succeedDice;

    /// <summary>
    /// Activate the Dice.
    /// </summary>
    public void Activate()
    {
        succeedDice = false;
        rollTimer?.Stop();

        parentObject.SetActive(true);
        SetRollStage(0);
    }

    /// <summary>
    /// Deactivate the Dice.
    /// </summary>
    public void Deactivate()
    {
        succeedDice = false;
        SetRollStage(0);
        parentObject.SetActive(false);
    }

    /// <summary>
    /// Roll the Dice.
    /// </summary>
    /// <param name="result">The result of the dice.</param>
    /// <param name="diceSuccess">Did the dice succeed.</param>
    /// <param name="rollDurationOffset">The offset duration of the roll animation. Used to create a better visual effect.</param>
    public void RollDice(int result, bool diceSuccess, float rollDurationOffset)
    {
        SetDiceSuccess(diceSuccess);
        SetRollStage(1);
        resultText.text = result.ToString();
        rollTimer = TimerManager.CreateRealTimer(rollDuration + rollDurationOffset, OnDiceRolled);
    }

    /// <summary>
    /// Called when the roll animation ends.
    /// </summary>
    public void OnDiceRolled()
    {
        SetRollStage(2);
    }

    /// <summary>
    /// Set the roll stage of the dice animation.
    /// </summary>
    /// <param name="toSet">The stage to set.</param>
    private void SetRollStage(int toSet)
    {
        rollStage = toSet;
        animator.SetInteger("RollStage", rollStage);
    }

    /// <summary>
    /// Set the success state of the dice animation.
    /// </summary>
    /// <param name="toSet"></param>
    private void SetDiceSuccess(bool toSet)
    {
        succeedDice = toSet;
        animator.SetBool("Success", succeedDice);
    }
}
