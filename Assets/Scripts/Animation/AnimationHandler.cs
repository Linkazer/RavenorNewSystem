using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [Header("Character Display")]
    [SerializeField] private Transform rendererTransform;

    [Header("Character Animations")]
    [SerializeField] private AnimationData[] animations;

    private AnimationData currentAnim;

    public void SetOrientation(Vector2 direction)
    {
        if (direction.x > 0)
        {
            rendererTransform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            rendererTransform.localEulerAngles = new Vector3(0, -180, 0);
        }
    }

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
}
