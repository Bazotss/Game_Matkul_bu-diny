using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;

    public float damage = 25f;

    Vector2 rightAttackOffset;

    void Start()
    {
        rightAttackOffset =
            transform.localPosition;

        swordCollider.enabled = false;
    }

    // =====================
    // ATTACK RIGHT
    // =====================
    public void AttackRight()
    {
        swordCollider.enabled = true;

        transform.localPosition =
            rightAttackOffset;
    }

    // =====================
    // ATTACK LEFT
    // =====================
    public void AttackLeft()
    {
        swordCollider.enabled = true;

        transform.localPosition =
            new Vector3(
                -rightAttackOffset.x,
                rightAttackOffset.y
            );
    }

    // =====================
    // STOP ATTACK
    // =====================
    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    // =====================
    // DAMAGE ENEMY
    // =====================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy =
                other.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(20);
            }
        }
    }
}