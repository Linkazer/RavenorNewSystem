using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationHandler : MonoBehaviour
{
    [Header("Character Display")]
    [SerializeField] private Transform rendererTransform;

    [Header("Character Animations")]
    [SerializeField] private AnimationData[] animations;

    [Header("Sorting Groups")]
    [SerializeField] private SortingGroup mainSortingGroup;
    [SerializeField] private SortingGroup frontHandSortingGroup;
    [SerializeField] private SortingGroup backHandSortingGroup;

    private AnimationData currentAnim;

    public void PlayAnimation(string animationName)
    {
        PlayAnimation(animationName, null);
    }

    public void PlayAnimation(string animationName, object animationData)
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

                    currentAnim.Play(animationData, () => PlayAnimation(currentAnim.LinkedAnimation, animationData));
                }
            }
        }
    }

    public void EndAnimation()
    {
        PlayAnimation("Idle");
        currentAnim = null;
    }

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
        PoolManager.InstatiatePoolableAtPosition(fxToPlay, transform.position, null);
    }
}
