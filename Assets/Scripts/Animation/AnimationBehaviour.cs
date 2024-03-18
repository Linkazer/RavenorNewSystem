using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationBehaviour : MonoBehaviour
{
    [SerializeField] private EC_Animator animatorComponent;

    protected AnimationData currentAnimation;

    public void Play(object animationEntryData, AnimationData animationToPlay)
    {
        currentAnimation = animationToPlay;

        Play(animationEntryData);
    }

    public abstract void Play(object animationdata);

    public abstract void Stop();

    public abstract void End();
}

public abstract class CharacterAnimation<T> : AnimationBehaviour
{
    protected T data;

    public override void Play(object animationdata)
    {
        if (animationdata.GetType() == typeof(T))
        {
            data = (T)animationdata;
            Play(data);
        }
    }

    public abstract void Play(T animationdata);
}
