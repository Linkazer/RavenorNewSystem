using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation Behavior for animations using the Animator.
/// </summary>
public class ANIMBH_Animator : AnimationBehaviour
{
    [SerializeField] protected Animator animator;

    private Timer animationTimer = null;

    public override void Play(object animationdata)
    {
        animator.Play($"Base Layer.{currentAnimation.AnimationName}");
        if (TimerManager.Instance != null)
        {
            animationTimer = TimerManager.CreateGameTimer(Time.deltaTime, SetAnimationLength);
        }
    }

    public override void End()
    {
        Stop();

        if (currentAnimation.LinkedAnimation != "")
        {
            currentAnimation.endCallback?.Invoke();
        }
    }

    public override void Stop()
    {
        animationTimer?.Stop();
        animationTimer = null;

        animator.Play("Base Layer.Character_Idle");
    }

    /// <summary>
    /// Set a timer to end the animation when animation is completed.
    /// </summary>
    private void SetAnimationLength()
    {
        if (!currentAnimation.DoesLoop)
        {
            TimerManager.CreateGameTimer(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - Time.deltaTime, End);
        }
    }
}
