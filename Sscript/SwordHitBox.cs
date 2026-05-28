using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    public int damage = 20;

    public float knockbackForce = 1f;
    private bool hasHit;
    private CameraShake camShake;

    void Start()
    {
        camShake =
            Camera.main.GetComponent<CameraShake>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        hasHit = false;
        if (other.CompareTag("Player"))
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy =
                other.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
