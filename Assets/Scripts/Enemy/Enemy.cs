using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : EnemyAnimator, IDamageable
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] int ashEchoesReward = 100;

    Rigidbody2D rigidBody;
    EnemyStateController stateController;
    Health health;
    bool isFacingLeft = true;


    public Attack Attack { get; private set; }
    public EnemyStateController Controller => stateController;
    public string Id { get; private set; }
    public int Reward => ashEchoesReward;

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
        EntityUtility.FlipSpriteHorizontally(transform, direction, ref isFacingLeft);
    }

    public void TakeDamage(int damage)
    {
        if (stateController.CurrentState is AttackState attackState)
        {
            attackState.InterruptAttack();
        }

        Play(Animations.DAMAGED, true, true);
        health.ApplyDamaage(damage);

        Debug.Log($"Got {damage} damage, remaining {health.CurrentHealth} HP.");
    }


    public void ResetFroomPool()
    {
        if (health != null)
            health.ResetHealth();

        EntityUtility.SetPhysicsEnabled(gameObject, true);
        Initiliaze(Animations.IDLE, GetComponentInChildren<Animator>(), DefaultAnimation);
        stateController.InitializeState();
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
}
