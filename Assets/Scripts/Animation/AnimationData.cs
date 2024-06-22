using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data d'une animation.
/// </summary>
[Serializable]
public class AnimationData
{
    /// <summary>
    /// Nom/ID de l'animation.
    /// </summary>
    [SerializeField] private string animationName;
    /// <summary>
    /// Type d'animation utilisé.
    /// </summary>
    [SerializeField] private AnimationBehaviour animationUsed;
    /// <summary>
    /// Est-ce que l'animation boucle ou non ?
    /// </summary>
    [SerializeField] private bool doesLoop;
    /// <summary>
    /// Animation à lancé une fois cette animation terminée. N'est pas utilisé sur les animations bouclant.
    /// </summary>
    [SerializeField] private string linkedAnimation = "";

    public Action endCallback;

    public string AnimationName => animationName;

    public string LinkedAnimation => linkedAnimation;

    public bool DoesLoop => doesLoop;

    /// <summary>
    /// Play the animation with some entry data.
    /// </summary>
    /// <param name="animationdata">Data that can be used by the animation.</param>
    /// <param name="callback">The callback to call at the end of the animation.</param>
    public void Play(object animationdata, Action callback)
    {
        endCallback = callback;

        animationUsed.Play(animationdata, this);
    }

    /// <summary>
    /// Stop the animation.
    /// </summary>
    public void Stop()
    {
        animationUsed.Stop();
    }
}
