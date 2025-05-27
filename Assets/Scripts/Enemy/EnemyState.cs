using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    public EnemyState nextState;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public virtual void Enter()
    {
        enabled = true;
        CancelInvoke();
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
