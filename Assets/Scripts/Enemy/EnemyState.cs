using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    public Enemy enemy;
    public EnemyState nextState;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public virtual void Enter()
    {
        enabled = true;
        CancelInvoke();
        Invoke(nameof(Exit), 10f);
    }

    public virtual void Exit()
    {
        enabled = false;
        CancelInvoke();
        if (nextState != null)
        {
            nextState.Enter();
        }
    }
}
