using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrcAI : MonoBehaviour
{
    [Header("Patrol")]
    

    private Transform patrolTarget;
    

    public float patrolSpeed = 1f;

    private bool isPatrolling;
    private Transform[] PatrolPoints;

    public float moveSpeed = 1f;
    public float chaseRange = 2f;
    public float attackRange = 0.6f;
    public LayerMask obstacleLayer;

    public float obstacleCheckDistance = 0.5f;

    public int damage = 10;
    public float attackCooldown = 1.5f;

    private float attackTimer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       



        GameObject[] points =
    GameObject.FindGameObjectsWithTag("PatrolPoint");

        PatrolPoints = new Transform[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            PatrolPoints[i] = points[i].transform;
        }

        ChooseRandomPoint();
    }

    void ChooseRandomPoint()
    {
        int random =
            Random.Range(0, PatrolPoints.Length);

        patrolTarget = PatrolPoints[random];
    }

    void Update()
    {
        FindTarget();

        if (target != null)
        {
            float distance =
                Vector2.Distance(transform.position,
                target.position);

            if (distance > attackRange)
            {
                ChaseTarget();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        anim.SetBool("isWalking", true);

        Vector2 direction =
            (patrolTarget.position -
            transform.position).normalized;

        direction = AvoidObstacle(direction);

        rb.velocity = direction * patrolSpeed;

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                patrolTarget.position
            );

        if (distance < 0.2f)
        {
            ChooseRandomPoint();
        }
    }

    void FindTarget()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        GameObject ally =
            GameObject.FindGameObjectWithTag("Ally");

        float playerDistance = Mathf.Infinity;
        float allyDistance = Mathf.Infinity;

        if (player != null)
        {
            playerDistance =
                Vector2.Distance(transform.position,
                player.transform.position);
        }

        if (ally != null)
        {
            allyDistance =
                Vector2.Distance(transform.position,
                ally.transform.position);
        }

        if (playerDistance <= chaseRange ||
           allyDistance <= chaseRange)
        {
            if (playerDistance < allyDistance)
            {
                target = player.transform;
            }
            else
            {
                target = ally.transform;
            }
        }
        else
        {
            target = null;
        }
    }

    void ChaseTarget()
    {
        Vector2 direction =
            (target.position - transform.position).normalized;

        direction = AvoidObstacle(direction);

        rb.velocity = direction * moveSpeed;

        anim.SetBool("isWalking", true);

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Attack()
    {
        rb.velocity = Vector2.zero;

        anim.SetBool("isWalking", false);

        if (Time.time >= attackTimer)
        {
            attackTimer = Time.time + attackCooldown;

            int combo = Random.Range(1, 3);

            if (combo == 1)
            {
                anim.SetTrigger("Attack1");
            }
            else
            {
                anim.SetTrigger("Attack2");
            }

            // Damage Player
            PlayerHealth playerHealth =
                target.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                Vector2 direction =
                    (target.position - transform.position).normalized;

                playerHealth.Knockback(direction, 0.5f);
            }
        }
    }

    void Idle()
    {
        rb.velocity = Vector2.zero;

        anim.SetBool("isWalking", false);
    }

    public void Die()
    {
        anim.SetBool("isDead", true);

        rb.velocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;

        this.enabled = false;

        Destroy(gameObject, 1.5f);
    }

    Vector2 AvoidObstacle(Vector2 direction)
    {
        RaycastHit2D hit =
            Physics2D.Raycast(
                transform.position,
                direction,
                obstacleCheckDistance,
                obstacleLayer
            );

        if (hit.collider != null)
        {
            Vector2 left =
                Quaternion.Euler(0, 0, 45) * direction;

            RaycastHit2D leftHit =
                Physics2D.Raycast(
                    transform.position,
                    left,
                    obstacleCheckDistance,
                    obstacleLayer
                );

            if (leftHit.collider == null)
            {
                return left.normalized;
            }

            Vector2 right =
                Quaternion.Euler(0, 0, -45) * direction;

            return right.normalized;
        }

        return direction;
    }
}
