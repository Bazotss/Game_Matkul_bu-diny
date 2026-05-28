using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 150;

    private int currentHealth;
    private Rigidbody2D rb;
    public GameObject coinPrefab;

    private Animator anim;
    private OrcAI orcAI;
    private bool Hurt;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        orcAI = GetComponent<OrcAI>();
    }

    public void Knockback(Vector2 direction, float force)
    {
        if (Hurt) return;

        StartCoroutine(DoKnockback(direction, force));
        IEnumerator DoKnockback(Vector2 direction, float force)
        {
            Hurt = true;

            rb.velocity = direction * force;

            yield return new WaitForSeconds(0.15f);

            rb.velocity = Vector2.zero;

            Hurt = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " kena damage");

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetBool("isDead", true);

        rb.velocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;

        OrcAI ai = GetComponent<OrcAI>();

        if (ai != null)
        {
            ai.enabled = false;
        }

        Instantiate(
            coinPrefab,
            transform.position,
            Quaternion.identity
        );

        Destroy(gameObject, 2f);
    }
}
