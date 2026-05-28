using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcSpawner : MonoBehaviour
{
    public GameObject orcPrefab;

    public float spawnRate = 5f;

    public int maxOrcs = 5;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;

            SpawnOrc();
        }
    }

    void SpawnOrc()
    {
        GameObject[] orcs =
            GameObject.FindGameObjectsWithTag("Enemy");

        if (orcs.Length >= maxOrcs)
        {
            return;
        }

        Instantiate(
            orcPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}
