using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SoldierAI : MonoBehaviour
{
    [Header("Follow Player")]
    private Transform player;
    public float followDistance = 0.8f;
    public float moveSpeed = 1f;

    [Header("Enemy Detection")]
    public float detectRange = 1f;
    public float attackRange = 1.2f;
    public LayerMask enemyLayer;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1f;

    private float lastAttackTime;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private Transform targetEnemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FindEnemy();

        if (targetEnemy != null)
        {
            float distance =
                Vector2.Distance(transform.position,
                targetEnemy.position);

            if (distance > attackRange)
            {
                FollowEnemy();
            }
            else
            {
                AttackEnemy();
            }
        }
        else
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            MoveTo(player.position);
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isWalking", false);
        }
    }

    void FollowEnemy()
    {
        Vector2 direction =
            (targetEnemy.position -
            transform.position).normalized;

        rb.velocity = direction * moveSpeed;

        anim.SetBool("isWalking", true);

        if (direction.x > 0)
        {
            sprite.flipX = false;
        }
        else if (direction.x < 0)
        {
            sprite.flipX = true;
        }
    }

    void MoveTo(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        rb.velocity = direction * moveSpeed;

        anim.SetBool("isWalking", true);

        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FindEnemy()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = Mathf.Infinity;

        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance =
                Vector2.Distance(transform.position,
                enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestEnemy = enemy.transform;
            }
        }

        if (closestDistance <= followDistance)
        {
            targetEnemy = closestEnemy;
        }
        else
        {
            targetEnemy = null;
        }
    }

    void AttackEnemy()
    {
        rb.velocity = Vector2.zero;

        anim.SetBool("isWalking", false);

        if (Time.time >= lastAttackTime)
        {
            lastAttackTime = Time.time + attackCooldown;

            int combo = Random.Range(1, 4);

            if (combo == 1)
            {
                anim.SetTrigger("Attack1");
            }
            else if (combo == 2)
            {
                anim.SetTrigger("Attack2");
            }
            else
            {
                anim.SetTrigger("Attack3");
            }

            EnemyHealth enemyHealth =
                targetEnemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
