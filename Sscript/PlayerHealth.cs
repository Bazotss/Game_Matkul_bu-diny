using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;

    private Animator anim;

    private Rigidbody2D rb;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        Debug.Log("Player kena damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Knockback(Vector2 direction, float force)
    {
        StartCoroutine(DoKnockback(direction, force));
    }

    IEnumerator DoKnockback(Vector2 direction, float force)
    {
        PlayerController player =
            GetComponent<PlayerController>();

        if (player != null)
        {
            player.LockMovement();
        }

        rb.velocity = direction * force;

        yield return new WaitForSeconds(0.08f);

        rb.velocity = Vector2.zero;

        if (player != null)
        {
            player.UnlockMovement();
        }
    }

    void Die()
    {
        if (anim != null)
        {
            anim.SetBool("Death", true);
        }

        PlayerController player =
            GetComponent<PlayerController>();

        if (player != null)
        {
            player.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
