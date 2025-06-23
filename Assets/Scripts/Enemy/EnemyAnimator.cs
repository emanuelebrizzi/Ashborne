using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    public enum AnimationState
    {
        IDLE,
        MOVE,
        ATTACK,
        DAMAGED,
        DEATH,
    }

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void PlayAnimation(AnimationState state)
    {
        if (animator == null) return;

        switch (state)
        {
            case AnimationState.IDLE:
                animator.SetBool("isMoving", false);
                break;

            case AnimationState.MOVE:
                animator.SetBool("isMoving", true);
                break;

            case AnimationState.ATTACK:
                animator.SetTrigger("isAttacking");
                break;

            case AnimationState.DAMAGED:
                animator.SetTrigger("isDamaged");
                break;

            case AnimationState.DEATH:
                animator.SetBool("isMoving", false);
                animator.ResetTrigger("isAttacking");
                animator.ResetTrigger("isDamaged");
                animator.SetBool("isDead", true);
                break;
        }
    }

    public void ResetAnimationState()
    {
        animator.SetBool("isDead", false);
        animator.SetBool("isMoving", false);
        animator.ResetTrigger("isAttacking");
        animator.ResetTrigger("isDamaged");
    }

    public float GetAnimationLength(AnimationState animation)
    {
        float defaultLength = 0.5f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animation.ToString()))
            return stateInfo.length;

        return defaultLength;
    }
}
