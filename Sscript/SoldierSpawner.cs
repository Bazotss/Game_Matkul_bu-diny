using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    public GameObject soldierPrefab;

    public float spawnRate = 8f;

    public int maxSoldiers = 3;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;

            SpawnSoldier();
        }
    }

    void SpawnSoldier()
    {
        GameObject[] soldiers =
            GameObject.FindGameObjectsWithTag("Ally");

        if (soldiers.Length >= maxSoldiers)
        {
            return;
        }

        Instantiate(
            soldierPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}
