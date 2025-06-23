using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(ChasingState))]
[RequireComponent(typeof(AttackState))]
[RequireComponent(typeof(DeathState))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] int ashEchoesReward = 100;
    [SerializeField] EnemyState initialState;

    Rigidbody2D rigidBody;
    EnemyAnimator enemyAnimator;
    Health health;
    bool isFacingLeft = true;
    EnemyState currentState;

    public PatrolState PatrolState { get; private set; }
    public ChasingState ChasingState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeathState DeathState { get; private set; }
    public Attack Attack { get; private set; }
    public EnemyAnimator Animator => enemyAnimator;
    public string Id { get; private set; }
    public int Reward => ashEchoesReward;

    public Transform PointA;
    public Transform PointB;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        Id = Guid.NewGuid().ToString();
        PatrolState = GetComponent<PatrolState>();
        ChasingState = GetComponent<ChasingState>();
        AttackState = GetComponent<AttackState>();
        DeathState = GetComponent<DeathState>();
    }

    void Start()
    {
        health = GetComponent<Health>();
        Attack = GetComponent<Attack>();

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

    public void MoveInDirection(float direction)
    {
        rigidBody.linearVelocityX = direction * speed;
        UpdateSpriteDirection(direction);
        enemyAnimator.PlayAnimation(EnemyAnimator.AnimationState.MOVE);
    }

    public void TakeDamage(int damage)
    {
        health.ApplyDamaage(damage);
        enemyAnimator.PlayAnimation(EnemyAnimator.AnimationState.DAMAGED);
        StartCoroutine(AfterDamageRoutine());

        Debug.Log($"Got {damage} damage, remaining {health.CurrentHealth} HP.");
    }

    private IEnumerator AfterDamageRoutine()
    {
        rigidBody.linearVelocity = Vector2.zero;
        float damageAnimationTime = enemyAnimator.GetAnimationLength(EnemyAnimator.AnimationState.DAMAGED);

        yield return new WaitForSeconds(damageAnimationTime);

        if (currentState == AttackState)
        {
            AttackState.CancelAttack();
        }
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

    public void ResetFroomPool()
    {
        if (health != null)
            health.ResetHealth();

        enemyAnimator.ResetAnimationState();

        SetPhysicElementsTo(true);
        ChangeState(initialState);
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

    public bool CanAttackPlayer()
    {
        return Vector2.Distance(transform.position, Player.Instance.transform.position) <= Attack.AttackRange;
    }

    public void Empower(float damageMultiplier, float healthMultiplier, float speedMultiplier)
    {
        ashEchoesReward += 500;
        speed *= speedMultiplier;
        Attack.IncreaseDamage(damageMultiplier);
        health.IncreaseMaxHealth(healthMultiplier);
        ChangeVisualAspect();

        Debug.Log($"Enemy {Id} empowered!");
    }

    void ChangeVisualAspect()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.color = new Color(1f, 0.6f, 0.6f);
        }

        transform.localScale *= 1.15f;
    }
}
