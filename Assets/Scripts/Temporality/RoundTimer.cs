using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimer
{
    public Timer timer;
    public float maximumRound;
    public float roundLeft;

    private Action onRoundUpdate;
    private Action onTimerEnd;

    /// <summary>
    /// Create a new RoundTimer.
    /// </summary>
    /// <param name="rounds">The duration in round.</param>
    /// <param name="updateCallback">The Action to call when a round is completed.</param>
    /// <param name="endCallback">The Action to call when the timer ends.</param>
    public RoundTimer(float rounds, Action updateCallback, Action endCallback)
    {
        maximumRound = rounds;
        roundLeft = maximumRound;

        onRoundUpdate = updateCallback;
        onTimerEnd = endCallback;
    }

    /// <summary>
    /// Start a real time Timer for the duration of a Round.
    /// </summary>
    public void StartRealTimer()
    {
        if (roundLeft <= 0)
        {
            TimerEnd();
            return;
        }

        float timerRoundDuration = RoundManager.RoundDuration;

        if (roundLeft < 1)
        {
            timerRoundDuration *= roundLeft;
        }

        timer = TimerManager.CreateGameTimer(timerRoundDuration, TimerRoundEnd);
    }

    /// <summary>
    /// Stop the current real time Timer if it is using one.
    /// </summary>
    public void StopRealTimer()
    {
        if (timer != null)
        {
            roundLeft -= timer.Duration - timer.DurationLeft;

            timer.Stop();
            timer = null;
        }
    }

    /// <summary>
    /// Progress the RoundTimer by a certain amount of Round.
    /// </summary>
    /// <param name="roundsToProgress">The number of Round to progress.</param>
    public void ProgressRound(float roundsToProgress)
    {
        roundLeft -= roundsToProgress;

        onRoundUpdate?.Invoke();

        if(roundLeft <= 0)
        {
            roundLeft = 0;
            TimerEnd();
        }
    }

    /// <summary>
    /// Called when the real time Timer end, progressing the RoundTimer of 1 Round.
    /// </summary>
    private void TimerRoundEnd()
    {
        ProgressRound(1);

        if(roundLeft > 0)
        {
            StartRealTimer();
        }
    }

    /// <summary>
    /// Called when the RoundTimer ends.
    /// </summary>
    private void TimerEnd()
    {
        onTimerEnd?.Invoke();

        RoundManager.Instance.RemoveTimer(this);
    }
}
