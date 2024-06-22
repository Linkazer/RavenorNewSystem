using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum RoundMode
{
    Round,
    RealTime,
}

/// <summary>
/// Gère tout ce qui est en lien avec les Tours de jeux (Début, Fin et fonctionnement en mode Temps réel)
/// </summary>
public class RoundManager : Singleton<RoundManager>
{
    public const float RoundDuration = 6f;

    private RoundMode currentRoundMode = RoundMode.Round;

    private List<RoundTimer> activeTimers = new List<RoundTimer>();

    private RoundTimer globalRoundTimer = null;

    private List<IRoundHandler> activeRoundHandlers = new List<IRoundHandler>();

    public Action<RoundMode> actOnUpdateRoundMode;

    public RoundMode CurrentRoundMode => currentRoundMode;

    private void Start()
    {
        SetRoundMode(RoundMode.RealTime);
    }

    /// <summary>
    /// Create a timer based on that update every Round.
    /// </summary>
    /// <param name="rounds">The duration of the timer in round.</param>
    /// <param name="updateCallback">The Action to call when a round is completed.</param>
    /// <param name="endCallback">The Action to call when the timer ends.</param>
    /// <returns>The timer created.</returns>
    public RoundTimer CreateRoundTimer(float rounds, Action updateCallback, Action endCallback)
    {
        RoundTimer toCreate = new RoundTimer(rounds, updateCallback, endCallback);

        if (CurrentRoundMode == RoundMode.RealTime)
        {
            toCreate.StartRealTimer();
        }

        activeTimers.Add(toCreate);

        return toCreate;
    }

    /// <summary>
    /// Remove a timer from the Active timer list.
    /// </summary>
    /// <param name="roundTimer">The timer to remove.</param>
    public void RemoveTimer(RoundTimer roundTimer)
    {
        activeTimers.Remove(roundTimer);
    }

    /// <summary>
    /// Add a IRoundHandler in the active list, making it possible to update every global round.
    /// </summary>
    /// <param name="toAdd">The IRoundManager to add.</param>
    public void AddHandler(IRoundHandler toAdd)
    {
        if(!activeRoundHandlers.Contains(toAdd))
        {
            activeRoundHandlers.Add(toAdd);
        }
    }

    /// <summary>
    /// Remove a IRoundHandler in the active list.
    /// </summary>
    /// <param name="toRemove">The IRoundManager to remove.</param>
    public void RemoveHandler(IRoundHandler toRemove)
    {
        if (activeRoundHandlers.Contains(toRemove))
        {
            activeRoundHandlers.Remove(toRemove);
        }
    }

    /// <summary>
    /// Set the RoundMode.
    /// </summary>
    /// <param name="toSet">The RoundMode to set.</param>
    public void SetRoundMode(RoundMode toSet)
    {
        if(currentRoundMode != toSet)
        {
            switch (toSet)
            {
                case RoundMode.RealTime:
                    if (globalRoundTimer == null)
                    {
                        globalRoundTimer = CreateRoundTimer(1f, null, OnGlobalRoundTimerEnd);
                    }

                    foreach (RoundTimer timer in activeTimers)
                    {
                        timer.StartRealTimer();
                    }
                    break;
                case RoundMode.Round:
                    if (globalRoundTimer != null)
                    {
                        RemoveTimer(globalRoundTimer);

                        globalRoundTimer.StopRealTimer();
                        globalRoundTimer = null;
                    }

                    foreach (RoundTimer timer in activeTimers)
                    {
                        timer.StopRealTimer();
                    }
                    break;
            }

            currentRoundMode = toSet;

            actOnUpdateRoundMode?.Invoke(currentRoundMode);
        }
    }

    /// <summary>
    /// Start a GlobalRound.
    /// </summary>
    public void StartGlobalRound()
    {
        foreach (IRoundHandler handler in activeRoundHandlers)
        {
            handler.StartRound();
        }
    }

    /// <summary>
    /// End a GlobalRound.
    /// </summary>
    public void EndGlobalRound()
    {
        foreach (IRoundHandler handler in activeRoundHandlers)
        {
            handler.EndRound();
        }
    }

    /// <summary>
    /// Called when the GlobalTimer complete a round.
    /// </summary>
    public void OnGlobalRoundTimerEnd()
    {
        EndGlobalRound();

        globalRoundTimer = CreateRoundTimer(1f, null, OnGlobalRoundTimerEnd);

        StartGlobalRound();
    }
}
