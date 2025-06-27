using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] EnemyState initialState;

    EnemyState currentState;

    public EnemyState CurrentState => currentState;
    public PatrolState PatrolState { get; private set; }
    public ChasingState ChasingState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeathState DeathState { get; private set; }

    void Awake()
    {
        PatrolState = GetComponent<PatrolState>();
        ChasingState = GetComponent<ChasingState>();
        AttackState = GetComponent<AttackState>();
        DeathState = GetComponent<DeathState>();
    }

    public void InitializeState()
    {
        if (initialState != null)
            ChangeState(initialState);
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.Tick();
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
            currentState.Enter();
    }
}
