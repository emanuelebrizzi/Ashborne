using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]
[RequireComponent(typeof(DeathState))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] int ashEchoesReward = 100;
    [SerializeField] EnemyState initialState;

    PatrolState patrolState;
    ChasingState chasingState;
    DeathState deathState;
    SPUM_Prefabs spumPrefabs;
    Health health;

    public const string LoggerTAG = "Enemy";
    public Logger MyLogger { get; private set; }
    public Rigidbody2D Body { get; private set; }
    public float Speed => speed;
    public int Reward => ashEchoesReward;

    void Start()
    {
        MyLogger = new Logger(Debug.unityLogger.logHandler);
        Body = GetComponent<Rigidbody2D>();
        spumPrefabs = GetComponent<SPUM_Prefabs>();
        patrolState = GetComponent<PatrolState>();
        chasingState = GetComponent<ChasingState>();
        deathState = GetComponent<DeathState>();
        health = GetComponent<Health>();
        spumPrefabs.OverrideControllerInit();

        ResetState();
    }

    void ResetState()
    {
        patrolState.Exit();
        chasingState.Exit();
        deathState.Exit();

        if (initialState != null)
        {
            initialState.Enter();
        }
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        MyLogger.Log(LoggerTAG, $"Got {damage} damage, remaining {health.CurrentHealth} HP.");

        if (health.CurrentHealth <= 0)
        {
            Die();
            return;
        }

        spumPrefabs.PlayAnimation(PlayerState.DAMAGED, 0);
    }

    void Die()
    {
        EnemyState currentState = null;

        if (patrolState.enabled)
            currentState = patrolState;
        else if (chasingState.enabled)
            currentState = chasingState;

        currentState.nextState = deathState;
        currentState.Exit();
    }

    // The assumption is that the sprite is facing left when x is positive
    public void UpdateSpriteDirection(float directionX)
    {
        if (directionX > 0)
        {
            // Moving right
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (directionX < 0)
        {
            // Moving left
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void PlayAnimation(PlayerState state, int index = 0)
    {
        if (spumPrefabs != null)
        {
            spumPrefabs.PlayAnimation(state, index);
        }
    }
}
