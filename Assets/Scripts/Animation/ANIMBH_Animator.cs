using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void SetAnimationLength()
    {
        if (!currentAnimation.DoesLoop)
        {
            TimerManager.CreateGameTimer(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - Time.deltaTime, End);
        }
    }
}
