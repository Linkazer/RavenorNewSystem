using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe abstraite utilisé pour les différents comportement d'une animation. Contient toute la logique lié à un type d'animation (Animator, Tween, ...)
/// </summary>
public abstract class AnimationBehaviour : MonoBehaviour
{
    protected AnimationData currentAnimation;

    /// <summary>
    /// Play an animation with some data.
    /// </summary>
    /// <param name="animationEntryData">The data for the animation.</param>
    /// <param name="animationToPlay">The animation to play.</param>
    public void Play(object animationEntryData, AnimationData animationToPlay)
    {
        currentAnimation = animationToPlay;

        Play(animationEntryData);
    }

    /// <summary>
    /// Play the animation.
    /// </summary>
    /// <param name="animationdata">The data for the animation.</param>
    public abstract void Play(object animationdata);

    /// <summary>
    /// Stop the animation.
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// Called when the animation ends.
    /// </summary>
    public abstract void End();
}

[System.Obsolete("Non utilisé. Obsolète ?")]//Pas forcément obsolète (Ex : Pour les animations d'attaque, on a besoin de savoir où est faite l'attaque)
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
