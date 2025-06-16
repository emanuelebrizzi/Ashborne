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
    // SPUM_Prefabs spumPrefabs;
    Animator animator;
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
        // spumPrefabs = GetComponent<SPUM_Prefabs>();
        patrolState = GetComponent<PatrolState>();
        chasingState = GetComponent<ChasingState>();
        deathState = GetComponent<DeathState>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        // spumPrefabs.OverrideControllerInit();

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

        PlayAnimation(PlayerState.DAMAGED, 0);
    }

    void Die()
    {

        // Cancel ALL pending state transitions
        EnemyState[] allStates = GetComponents<EnemyState>();
        foreach (var state in allStates)
        {
            if (state != deathState)
            {
                state.enabled = false;  // Disable all non-death states
            }
        }

        // Force enter death state directly
        deathState.Enter();
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
        if (animator == null) return;

        MyLogger.Log(LoggerTAG, $"Playing animation: {state}");

        switch (state)
        {
            case PlayerState.IDLE:
                animator.SetBool("isMoving", false);
                animator.ResetTrigger("isAttacking");
                animator.ResetTrigger("isDamaged");
                break;

            case PlayerState.MOVE:
                animator.SetBool("isMoving", true);
                break;

            case PlayerState.ATTACK:
                animator.SetTrigger("isAttacking");
                break;

            case PlayerState.DAMAGED:
                animator.SetTrigger("isDamaged");
                break;

            case PlayerState.DEATH:
                animator.SetBool("isDead", true);
                break;
        }
    }
}
