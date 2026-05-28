using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHealth : MonoBehaviour
{
    [Header("Health")]
    public float health = 100;

    bool isDead = false;

    Animator animator;

    SoldierAI ai;

    Rigidbody2D rb;

    void Start()
    {
        animator =
            GetComponent<Animator>();

        ai =
            GetComponent<SoldierAI>();

        rb =
            GetComponent<Rigidbody2D>();
    }

    // =====================
    // TAKE DAMAGE
    // =====================
    public void TakeDamage(
        float damage
    )
    {
        if (isDead)
            return;

        health -= damage;

        animator.SetTrigger(
            "Hurt"
        );

        Debug.Log(
            "Soldier Health: " +
            health
        );

        if (health <= 0)
        {
            Die();
        }
    }

    // =====================
    // DIE
    // =====================
    void Die()
    {
        isDead = true;

        rb.velocity = Vector2.zero;

        // Matikan AI
        if (ai != null)
        {
            ai.enabled = false;
        }

        animator.SetBool(
            "isMoving",
            false
        );

        animator.SetTrigger(
            "Death"
        );

        // Matikan collider
        Collider2D col =
            GetComponent<Collider2D>();

        if (col != null)
        {
            col.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
