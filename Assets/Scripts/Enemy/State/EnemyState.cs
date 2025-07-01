using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public virtual void Enter()
    {
        enabled = true;
    }

    public virtual void Exit()
    {
        enabled = false;
    }

    public abstract void Update();
}
