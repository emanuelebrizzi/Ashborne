using System;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    readonly static int[] animations = {
        Animator.StringToHash("IDLE"),
        Animator.StringToHash("MOVE"),
        Animator.StringToHash("ATTACK"),
        Animator.StringToHash("DAMAGED"),
        Animator.StringToHash("DEATH")
    };

    Animations currentAnimation;
    bool layerLocked;
    Action DefaultAnimation;

    protected void Initiliaze(Animations startingAnimation, Animator animator, Action DefaultAnimation)
    {
        layerLocked = false;
        currentAnimation = startingAnimation;
        this.animator = animator;
        this.DefaultAnimation = DefaultAnimation;
    }

    public Animations CurrentAnimation => currentAnimation;

    public void SetLocked(bool lockLayer) => layerLocked = lockLayer;

    public virtual void Play(Animations animation, bool lockLayer, bool bypassLock, float crossFade = 0.2f)
    {
        if (animation == Animations.NONE)
        {
            DefaultAnimation();
            return;
        }

        if (lockLayer && !bypassLock) return;

        layerLocked = lockLayer;


        if (bypassLock)
        {
            foreach (var item in animator.GetBehaviours<OnExit>())
            {
                item.cancel = true;
            }
        }


        if (currentAnimation == animation) return;

        currentAnimation = animation;
        animator.CrossFade(animations[(int)currentAnimation], crossFade, 0);
    }
}

public enum Animations
{
    IDLE,
    MOVE,
    ATTACK,
    DAMAGED,
    DEATH,
    NONE
}
