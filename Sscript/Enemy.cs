using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

    [Header("Health")]
    public float health = 100;

    [Header("Knockback")]
    public float knockbackForce = 1f;

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab;

    bool isDead = false;

    public float Health
    {
        set
        {
            health = value;

            if (health <= 0 && !isDead)
            {
                Defeated();
            }
        }

        get
        {
            return health;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

        // Supaya slime stabil
        rb.gravityScale = 0;

        rb.freezeRotation = true;
    }

    // Fungsi kena damage
    public void OnHit(
        float damage,
        Vector2 hitDirection
    )
    {
        if (isDead)
            return;

        Health -= damage;

        Debug.Log(
            "Health Sekarang: " + health
        );

        // Reset velocity dulu
        rb.velocity = Vector2.zero;

        // Knockback kecil
        rb.AddForce(
            hitDirection * knockbackForce,
            ForceMode2D.Impulse
        );

        // Damage popup
        if (damagePopupPrefab != null)
        {
            GameObject popup =
                Instantiate(
                    damagePopupPrefab,
                    transform.position,
                    Quaternion.identity
                );

            popup.GetComponent<DamagePopup>()
                .Setup((int)damage);
        }

        // Animasi hit
        animator.SetTrigger("Hit");
    }

    public void Defeated()
    {
        isDead = true;

        // Stop gerak
        rb.velocity = Vector2.zero;

        // Matikan AI
        SlimeAI ai =
            GetComponent<SlimeAI>();

        if (ai != null)
        {
            ai.enabled = false;
        }

        // Disable collider
        Collider2D col =
            GetComponent<Collider2D>();

        if (col != null)
        {
            col.enabled = false;
        }

        // Animasi mati
        animator.SetTrigger("Defeated");

        Destroy(gameObject, 2f);
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}
