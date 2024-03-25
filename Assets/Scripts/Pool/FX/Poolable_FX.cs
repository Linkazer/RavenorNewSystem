using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Poolable_FX : MonoBehaviour
{
    public enum ActionOnEnd
    {
        Destroy,
        Disable,
        DesactivateObject,
        None,
    }

    /// The lifespan of the animation.
    [SerializeField] private float playTime;
    [SerializeField] private float callbackDelay;
    [SerializeField] private ActionOnEnd endAction = ActionOnEnd.Destroy;

    public float PlayTime => playTime;
    public float CallbackDelay => callbackDelay;

    private Action endCallback;

    private Timer animationTimer = null;

    [SerializeField] private UnityEvent<float> onSetTime;

    private void OnDestroy()
    {
        animationTimer?.Stop();
    }

    /// <summary>
    /// Play the animation.
    /// </summary>
    /// <param name="callback">The callback to call at the end of the animation.</param>
    public void Play(Action callback)
    {
        endCallback = callback;

        if (playTime > 0)
        {
            animationTimer = TimerManager.CreateGameTimer(callbackDelay, CallCallback);
        }
    }

    private void CallCallback()
    {
        endCallback?.Invoke();

        animationTimer = TimerManager.CreateGameTimer(PlayTime - callbackDelay, End);
    }

    /// <summary>
    /// End the animation.
    /// </summary>
    public void End()
    {
        switch (endAction)
        {
            case ActionOnEnd.Destroy:
                Destroy(gameObject);
                break;
            case ActionOnEnd.Disable:
                enabled = false;
                break;
            case ActionOnEnd.DesactivateObject:
                gameObject.SetActive(false);
                break;
            case ActionOnEnd.None:
                break;
        }
    }
}
