using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] int ashEchoesReward = 100;

    Rigidbody2D rigidBody;
    EnemyAnimator enemyAnimator;
    EnemyStateController stateController;
    Health health;
    bool isFacingLeft = true;

    public Attack Attack { get; private set; }
    public EnemyAnimator Animator => enemyAnimator;
    public EnemyStateController Controller => stateController;
    public string Id { get; private set; }
    public int Reward => ashEchoesReward;

    public Transform PointA;
    public Transform PointB;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        stateController = GetComponent<EnemyStateController>();
        Id = Guid.NewGuid().ToString();
    }

    void Start()
    {
        health = GetComponent<Health>();
        Attack = GetComponent<Attack>();

        health.OnDeath += Die;

        enemyAnimator.InitializeAnimationStates();
        stateController.InitializeState();
    }

    void Update()
    {
        stateController.UpdateState();
    }

    void Die()
    {
        stateController.ChangeState(stateController.DeathState);
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

        if (stateController.IsInAttackState())
        {
            stateController.AttackState.CancelAttack();
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

        SetPhysicElementsTo(true);
        enemyAnimator.InitializeAnimationStates();
        stateController.InitializeState();
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
