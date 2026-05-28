using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcEnemy : MonoBehaviour
{
    [Header("Health")]
    public float health = 100;

    bool isDead = false;

    Animator animator;

    OrcAI ai;

    void Start()
    {
        animator =
            GetComponent<Animator>();

        ai =
            GetComponent<OrcAI>();
    }

    public void OnHit(
        float damage,
        Vector2 hitDirection
    )
    {
        if (isDead)
            return;

        health -= damage;

        animator.SetTrigger(
            "Hurt"
        );

        Debug.Log(
            "Orc Health: " + health
        );

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (ai != null)
        {
            ai.Die();
        }
    }
}
