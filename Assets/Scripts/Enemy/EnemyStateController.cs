using System;
using UnityEngine;


public class EnemyStateController : MonoBehaviour
{
    public EnemyState CurrentState { get; private set; }
    public PatrolState PatrolState { get; private set; }
    public ChasingState ChasingState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeathState DeathState { get; private set; }

    public event Action<EnemyState> StateChanged;

    void Awake()
    {
        PatrolState = GetComponent<PatrolState>();
        ChasingState = GetComponent<ChasingState>();
        AttackState = GetComponent<AttackState>();
        DeathState = GetComponent<DeathState>();
    }

    public void Initialize(EnemyState state)
    {
        CurrentState = state;
        state.Enter();

        StateChanged?.Invoke(state);
    }

    public void TransitionTo(EnemyState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();

        StateChanged?.Invoke(nextState);
    }
}
