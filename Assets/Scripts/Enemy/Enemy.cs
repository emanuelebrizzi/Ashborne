using System;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : EnemyAnimator, IDamageable
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] int ashEchoesReward = 100;
    [SerializeField] float detectionRange = 3f;
    [SerializeField] float maxChaseDistance = 6.0f;
    [SerializeField] EnemyState initialState;

    Rigidbody2D rigidBody;
    EnemyStateController stateController;
    Health health;
    bool isFacingLeft = true;

    public Attack Attack { get; private set; }
    public EnemyStateController StateController => stateController;
    public string Id { get; private set; }

    public Transform PointA;
    public Transform PointB;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        stateController = GetComponent<EnemyStateController>();
        Id = Guid.NewGuid().ToString();
    }

    void Start()
    {
        health = GetComponent<Health>();
        Attack = GetComponent<Attack>();

        health.OnDeath += Die;

        stateController.Initialize(initialState);
        Initiliaze(Animations.IDLE, GetComponentInChildren<Animator>(), DefaultAnimation);
    }

    public void TakeDamage(int damage)
    {
        if (stateController.CurrentState is AttackState attackState)
        {
            attackState.InterruptAttack();
        }

        Play(Animations.DAMAGED, true, false);
        health.ApplyDamaage(damage);

        Debug.Log($"Got {damage} damage, remaining {health.CurrentHealth} HP.");
    }

    void Die()
    {
        stateController.TransitionTo(stateController.DeathState);
    }

    public void ResetFromPool()
    {
        if (health != null)
            health.ResetHealth();

        EntityUtility.SetPhysicsEnabled(gameObject, true);

        stateController.TransitionTo(initialState);
        Initiliaze(Animations.IDLE, GetComponentInChildren<Animator>(), DefaultAnimation);
    }

    public bool CanAttackPlayer()
    {
        return Vector2.Distance(transform.position, Player.Instance.transform.position) <= Attack.AttackRange;
    }

    public void Empower(float damageMultiplier, float healthMultiplier, float speedMultiplier)
    {
        ashEchoesReward += 500;
        speed *= speedMultiplier;
        Attack.IncreaseAttackDamage(Mathf.RoundToInt(Attack.AttackDamage * damageMultiplier));
        health.IncreaseMaxHealth(Mathf.RoundToInt(health.CurrentHealth * healthMultiplier));
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

    public void CheckMovementAnimation()
    {
        if (rigidBody.linearVelocityX != 0)
        {
            Play(Animations.MOVE, false, false);
        }
        else
        {
            Play(Animations.IDLE, false, false);
        }
    }

    public void DefaultAnimation()
    {
        CheckMovementAnimation();
    }

    public void MoveToward(Transform target)
    {
        float direction = (target.position - transform.position).normalized.x;
        rigidBody.linearVelocityX = direction * speed;
        EntityUtility.FlipSpriteHorizontally(transform, direction, ref isFacingLeft);
    }

    public float DistanceTo(Transform target)
    {
        return Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(target.position.x, 0));
    }

    public bool HasDetectedPlayer()
    {
        if (Player.Instance == null) return false;

        if (DistanceTo(Player.Instance.transform) > detectionRange) return false;

        return HasHittedThePlayer();
    }

    bool HasHittedThePlayer()
    {
        Vector2 raycastOrigin = (Vector2)transform.position + Vector2.up * 0.5f;
        Vector2 directionToPlayer = (Player.Instance.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
            raycastOrigin,
            directionToPlayer,
            detectionRange,
            LayerMask.GetMask("Player")
        );

        if (hit.collider != null)
        {
            return hit.transform == Player.Instance.transform;
        }
        else
        {
            return false;
        }
    }

    public bool IsOutOfRange(Transform startingPoint)
    {
        return DistanceTo(Player.Instance.transform) > maxChaseDistance || DistanceTo(startingPoint) > maxChaseDistance;
    }

    public bool IsNearPointA(float minimumDistance)
    {
        return Vector2.Distance(transform.position, PointA.position) < minimumDistance;
    }

    public bool IsNearPointB(float minimumDistance)
    {
        return Vector2.Distance(transform.position, PointB.position) < minimumDistance;
    }

    public void AwardAshEchoes()
    {
        if (Player.Instance != null)
        {
            Player.Instance.AddAshEchoes(ashEchoesReward);
            Debug.Log($"Awarded {ashEchoesReward} Ash Echoes to player");
        }
    }
}
