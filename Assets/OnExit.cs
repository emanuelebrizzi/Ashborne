using System.Collections;
using UnityEngine;

public class OnExit : StateMachineBehaviour
{
    [SerializeField] Animations animation;
    [SerializeField] bool lockLayer;
    [SerializeField] float crossFade = 0.2f;
    public bool cancel = false;

    Enemy enemy;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cancel = false;
        enemy = animator.GetComponentInParent<Enemy>();
        enemy.StartCoroutine(Wait());

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(stateInfo.length - crossFade);

            if (cancel) yield break;

            EnemyAnimator target = animator.GetComponentInParent<EnemyAnimator>();
            target.SetLocked(false);
            target.Play(animation, lockLayer, false, crossFade);
        }
    }
}
