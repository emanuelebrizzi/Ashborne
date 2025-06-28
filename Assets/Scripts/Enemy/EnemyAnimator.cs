using System;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    /*  This class helps to manage animations programmatically. 
        The classes that want to control the animations of the sprite, need to extend this class and 
        to implement the Unity Action for the default animation. 
        It uses an array to remeber the animations to play.
    */

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

    public void Play(Animations animation, bool lockLayer, bool bypassLock, float crossFade = 0.2f)
    {
        if (animation == Animations.NONE)
        {
            DefaultAnimation();
            return;
        }

        if (layerLocked && !bypassLock) return;

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
    NONE    // This is used when we want to return to the default animaton
}
