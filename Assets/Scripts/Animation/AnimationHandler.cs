using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationHandler : MonoBehaviour
{
    [Header("Character Display")]
    [SerializeField] private Transform rendererTransform;

    [Header("Animations Data")]
    [SerializeField] private Animator animator;

    [Header("Character Animations")]
    [SerializeField] private AnimationData[] animations;

    [Header("Sorting Groups")]
    [SerializeField] private SortingGroup mainSortingGroup;
    [SerializeField] private SortingGroup frontHandSortingGroup;
    [SerializeField] private SortingGroup backHandSortingGroup;

    [Header("Outline")]
    [SerializeField] private Outline outline;

    private AnimationData currentAnim;

    private Action[] animationCallbacks;

    public void SetOutline(bool toSet)
    {
        if (outline != null)
        {
            if (toSet)
            {
                outline.SetOutline();
            }
            else
            {
                outline.UnsetOutline();
            }
        }
    }

    public void SetOutline(Color colorWanted)
    {
        if (outline != null)
        {
            outline.SetSpecialOutline(colorWanted);
        }
    }

    /// <summary>
    /// Play an animation.
    /// </summary>
    /// <param name="animationName">The Name/ID of the animation to play.</param>
    public void PlayAnimation(string animationName, Action[] callbacks)
    {
        PlayAnimation(animationName, callbacks, null);
    }

    /// <summary>
    /// Play an animation with specific data.
    /// </summary>
    /// <param name="animationName">The Name/ID of the animation to play.</param>
    /// <param name="animationData">The data for the animation.</param>
    public void PlayAnimation(string animationName, Action[] callbacks, object animationData)
    {
        if (currentAnim == null || currentAnim.AnimationName != animationName)
        {
            foreach (AnimationData anim in animations)
            {
                if (animationName == anim.AnimationName)
                {
                    if (currentAnim != null)
                    {
                        currentAnim.Stop();
                    }

                    currentAnim = anim;

                    animationCallbacks = callbacks;

                    currentAnim.Play(animationData, () => PlayAnimation(currentAnim.LinkedAnimation, callbacks, animationData));
                }
            }
        }
    }

    /// <summary>
    /// Set the animation to the base animation (Idle)
    /// </summary>
    public void EndAnimation()
    {
        PlayAnimation("Idle", null);
        currentAnim = null;
    }

    public void SetOrientation(Vector2 orientation)
    {
        animator.SetFloat("OrientationX", orientation.x);
        animator.SetFloat("OrientationY", orientation.y);
    }

    /// Methods that can be used in the Animator.
    #region Animator Methods
    public void ANIM_UpdateMainSortingGroup(int sortingOrder)
    {
        mainSortingGroup.sortingOrder = sortingOrder;
    }

    public void ANIM_UpdateFrontHandSortingGroup(int sortingOrder)
    {
        frontHandSortingGroup.sortingOrder = sortingOrder;
    }

    public void ANIM_UpdateBackHandSortingGroup(int sortingOrder)
    {
        backHandSortingGroup.sortingOrder = sortingOrder;
    }

    public void ANIM_PlayFX(Poolable_FX fxToPlay)
    {
        PoolManager.InstantiatePoolableAtPosition(fxToPlay, transform.position);
    }

    public void ANIM_CALLBACK_PlayCallback(int callbackIndex)
    {
        if(animationCallbacks != null)
        {
            if(callbackIndex < animationCallbacks.Length)
            {
                animationCallbacks[callbackIndex]?.Invoke();
            }
        }
    }
    #endregion
}
