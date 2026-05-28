using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcMovement : MonoBehaviour
{
    public float speed = 1f;
    public Transform[] patrolPoints;
    public float waitTime = 2f;

    private int currentPointIndex = 0;
    private bool isWaiting = false;

    void Update()
    {
        if (patrolPoints.Length == 0) return;

        if (!isWaiting)
        {
            MoveToPoint();
        }
    }

    void MoveToPoint()
    {
        Transform target = patrolPoints[currentPointIndex];

        // Gerak ke target
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Cek sudah sampai atau belum
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        // Pindah ke titik berikutnya
        currentPointIndex++;

        if (currentPointIndex >= patrolPoints.Length)
        {
            currentPointIndex = 0; // ulang dari awal
        }

        FlipSprite();

        isWaiting = false;
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}