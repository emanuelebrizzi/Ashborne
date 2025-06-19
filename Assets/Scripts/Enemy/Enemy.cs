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

    Animator animator;
    Health health;

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


    void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

        patrolState = GetComponent<PatrolState>();
        chasingState = GetComponent<ChasingState>();
        deathState = GetComponent<DeathState>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        PlayerMask = LayerMask.GetMask("Player");

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

    public void MoveInDirection(float direction)
    {

        Body.linearVelocityX = direction * speed;
        UpdateSpriteDirection(direction);
        PlayAnimation(AnimationState.MOVE);
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        Debug.Log($"Got {damage} damage, remaining {health.CurrentHealth} HP.");

        if (health.CurrentHealth <= 0)
        {
            Die();
            return;
        }

        PlayAnimation(AnimationState.DAMAGED);
    }

    void Die()
    {
        EnemyState[] allStates = GetComponents<EnemyState>();
        foreach (var state in allStates)
        {
            if (state != deathState)
            {
                state.enabled = false;
            }
        }

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

    public void PlayAnimation(AnimationState state)
    {
        if (animator == null) return;

        switch (state)
        {
            case AnimationState.IDLE:
                animator.SetBool("isMoving", false);
                animator.ResetTrigger("isAttacking");
                animator.ResetTrigger("isDamaged");
                break;

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
                animator.SetBool("isDead", true);
                break;
        }
    }
}
