using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 2f;

    [Header("Detection")]
    public float detectionRange = 1f;
    public LayerMask obstacleLayer;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currentPoint = 0;

    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // Deteksi player
        if (distance <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement;
    }

    
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Raycast untuk cek tembok
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            1f,
            obstacleLayer
        );

        // Jika ada tembok
        if (hit.collider != null)
        {
            // Cari jalan alternatif
            Vector2 leftPath = Quaternion.Euler(0, 0, 45) * direction;
            Vector2 rightPath = Quaternion.Euler(0, 0, -45) * direction;

            bool leftBlocked = Physics2D.Raycast(transform.position, leftPath, 1f, obstacleLayer);
            bool rightBlocked = Physics2D.Raycast(transform.position, rightPath, 1f, obstacleLayer);

            if (!leftBlocked)
            {
                movement = leftPath * chaseSpeed;
            }
            else if (!rightBlocked)
            {
                movement = rightPath * chaseSpeed;
            }
            else
            {
                movement = Vector2.zero;
            }
        }
        else
        {
            movement = direction * chaseSpeed;
        }
    }

    
    void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPoint];

        Vector2 direction = (targetPoint.position - transform.position).normalized;

        movement = direction * moveSpeed;

        float distance = Vector2.Distance(transform.position, targetPoint.position);

        // Ganti titik patrol
        if (distance < 0.3f)
        {
            currentPoint++;

            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}