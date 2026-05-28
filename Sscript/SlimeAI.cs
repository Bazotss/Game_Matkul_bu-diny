using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 1f;

    [Header("Detection")]
    public float chaseRange = 0.5f;

    [Header("Attack")]
    public float attackRange = 0.5f;

    public int damage = 10;

    public float attackCooldown = 1.5f;

    float attackTimer;

    [Header("Patrol")]
    public float patrolDistance = 2f;

    public float patrolWaitTime = 2f;

    Vector2 startPosition;
    Vector2 patrolTarget;

    bool isWaiting = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Cari player otomatis
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        startPosition = transform.position;

        ChooseNewPatrolPoint();
    }

    void Update()
    {
        if (player == null)
            return;

        attackTimer += Time.deltaTime;

        float distanceToPlayer =
            Vector2.Distance(
                transform.position,
                player.position
            );

        // =========================
        // CHASE PLAYER
        // =========================
        if (
            distanceToPlayer <= chaseRange &&
            distanceToPlayer > attackRange
        )
        {
            ChasePlayer();
        }

        // =========================
        // ATTACK PLAYER
        // =========================
        else if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }

        // =========================
        // PATROL
        // =========================
        else
        {
            Patrol();
        }
    }

    // =========================
    // CHASE
    // =========================
    void ChasePlayer()
    {
        transform.position =
            Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );

        animator.SetBool(
            "IsMoving",
            true
        );

        FlipSprite(
            player.position.x -
            transform.position.x
        );
    }

    // =========================
    // ATTACK
    // =========================
    void AttackPlayer()
    {
        animator.SetBool(
            "IsMoving",
            false
        );

        // Hadap player
        FlipSprite(
            player.position.x -
            transform.position.x
        );

        // Cooldown attack
        if (attackTimer >= attackCooldown)
        {
            animator.SetTrigger("Attack");

            PlayerHealth playerHealth =
                player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(
                    damage
                );
            }

            attackTimer = 0f;
        }
    }

    // =========================
    // PATROL
    // =========================
    void Patrol()
    {
        if (isWaiting)
            return;

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                patrolTarget,
                moveSpeed * Time.deltaTime
            );

        animator.SetBool(
            "IsMoving",
            true
        );

        FlipSprite(
            patrolTarget.x -
            transform.position.x
        );

        float distance =
            Vector2.Distance(
                transform.position,
                patrolTarget
            );

        if (distance < 0.1f)
        {
            animator.SetBool(
                "IsMoving",
                false
            );

            StartCoroutine(
                WaitAndChooseNewPoint()
            );
        }
    }

    IEnumerator WaitAndChooseNewPoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(
            patrolWaitTime
        );

        ChooseNewPatrolPoint();

        isWaiting = false;
    }

    void ChooseNewPatrolPoint()
    {
        patrolTarget =
            startPosition +
            Random.insideUnitCircle *
            patrolDistance;
    }

    // =========================
    // FLIP SPRITE
    // =========================
    void FlipSprite(float directionX)
    {
        if (directionX > 0)
        {
            transform.localScale =
                new Vector3(1, 1, 1);
        }
        else if (directionX < 0)
        {
            transform.localScale =
                new Vector3(-1, 1, 1);
        }
    }

    // =========================
    // GIZMOS
    // =========================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            chaseRange
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}