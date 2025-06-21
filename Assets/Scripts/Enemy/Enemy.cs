using System;
using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]
[RequireComponent(typeof(DeathState))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] int ashEchoesReward = 100;
    [SerializeField] EnemyState initialState;

    EnemyState currentState;
    public PatrolState PatrolState { get; private set; }
    public ChasingState ChasingState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeathState DeathState { get; private set; }

    Animator animator;
    Health health;

    public string Id { get; private set; }
    public LayerMask PlayerMask { get; private set; }
    public Rigidbody2D Body { get; private set; }
    public int Reward => ashEchoesReward;
    public enum AnimationState
    {
        IDLE,
        MOVE,
        ATTACK,
        DAMAGED,
        DEATH,
    }
    bool isFacingLeft = true;

    public EnemySpawnManager.SpawnPoint MySpawnPoint;
    public Transform PointA;
    public Transform PointB;


    void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        Id = Guid.NewGuid().ToString();
        PatrolState = GetComponent<PatrolState>();
        ChasingState = GetComponent<ChasingState>();
        AttackState = GetComponent<AttackState>();
        DeathState = GetComponent<DeathState>();
    }
    void Start()
    {
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        PlayerMask = LayerMask.GetMask("Player");

        health.OnDeath += Die;

        ChangeState(initialState);
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    void Update()
    {
        currentState.Tick();
    }

    public void MoveInDirection(float direction)
    {
        Body.linearVelocityX = direction * speed;
        UpdateSpriteDirection(direction);
        PlayAnimation(AnimationState.MOVE);
    }

    public void TakeDamage(int damage)
    {
        health.ApplyDamaage(damage);
        PlayAnimation(AnimationState.DAMAGED);
        Debug.Log($"Got {damage} damage, remaining {health.CurrentHealth} HP.");
    }



    void Die()
    {
        ChangeState(DeathState);
    }

    public void UpdateSpriteDirection(float directionX)
    {
        if (directionX > 0 && isFacingLeft)
        {
            Flip();
        }
        else if (directionX < 0 && !isFacingLeft)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        isFacingLeft = !isFacingLeft;
    }

    public void PlayAnimation(AnimationState state)
    {
        if (animator == null) return;

        switch (state)
        {
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


    public void ResetFroomPool()
    {
        if (health != null)
            health.ResetHealth();


        if (animator != null)
        {
            InitializeAnimatorVariables();
        }

        SetPhysicElementsTo(true);
        ChangeState(initialState);
    }

    void InitializeAnimatorVariables()
    {
        animator.SetBool("isDead", false);
        animator.SetBool("isMoving", false);
        animator.ResetTrigger("isAttacking");
        animator.ResetTrigger("isDamaged");
    }

    public void SetPhysicElementsTo(bool value)
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = value;
        }

        if (Body != null)
        {
            Body.simulated = value;
        }
    }

    public void SetPatrolPoints(Transform pointA, Transform pointB)
    {
        PointA = pointA;
        PointB = pointB;
    }
}
