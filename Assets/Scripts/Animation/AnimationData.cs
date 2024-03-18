using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationData
{
    [SerializeField] private string animationName;
    [SerializeField] private AnimationBehaviour animationUsed;
    [SerializeField] private bool doesLoop;
    [SerializeField] private string linkedAnimation = "";

    public Action endCallback;

    public string AnimationName => animationName;
    public string LinkedAnimation => linkedAnimation;

    public bool DoesLoop => doesLoop;

    public void Play(object animationdata, Action callback)
    {
        endCallback = callback;

        animationUsed.Play(animationdata, this);
    }

    public void Stop()
    {
        animationUsed.Stop();
    }
}
