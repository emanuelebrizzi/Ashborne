using System;
using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]
[RequireComponent(typeof(AttackState))]
[RequireComponent(typeof(DeathState))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed = 3.0f;
    [SerializeField] protected int ashEchoesReward = 100;
    [SerializeField] protected EnemyState initialState;

    protected Rigidbody2D rigidBody;
    protected Animator animator;
    protected Health health;
    protected bool isFacingLeft = true;
    protected EnemyState currentState;

    public PatrolState PatrolState { get; private set; }
    public ChasingState ChasingState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeathState DeathState { get; private set; }
    public Attack AttackBehaviour { get; private set; }
    public string Id { get; private set; }
    public int Reward => ashEchoesReward;
    public Animator Animator => animator;
    public enum AnimationState
    {
        IDLE,
        MOVE,
        ATTACK,
        DAMAGED,
        DEATH,
    }

    public Transform PointA;
    public Transform PointB;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Id = Guid.NewGuid().ToString();
        PatrolState = GetComponent<PatrolState>();
        ChasingState = GetComponent<ChasingState>();
        AttackState = GetComponent<AttackState>();
        DeathState = GetComponent<DeathState>();
    }

    void Start()
    {
        health = GetComponent<Health>();
        AttackBehaviour = GetComponent<Attack>();
        animator = GetComponentInChildren<Animator>();

        health.OnDeath += Die;

        ChangeState(initialState);
    }

    void Die()
    {
        ChangeState(DeathState);
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

    public virtual void MoveInDirection(float direction)
    {
        rigidBody.linearVelocityX = direction * speed;
        UpdateSpriteDirection(direction);
        PlayAnimation(AnimationState.MOVE);
    }

    public void TakeDamage(int damage)
    {
        health.ApplyDamaage(damage);
        PlayAnimation(AnimationState.DAMAGED);
        Debug.Log($"Got {damage} damage, remaining {health.CurrentHealth} HP.");
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

        if (rigidBody != null)
        {
            rigidBody.simulated = value;
        }
    }
}
