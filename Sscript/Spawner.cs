using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public int maxEnemies = 10;

    [Header("Spawn Area")]
    public Transform spawnArea;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Hitung jumlah enemy aktif
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Jika sudah penuh
        if (enemies.Length >= maxEnemies)
            return;

        // Ambil ukuran area spawn
        Vector2 center = spawnArea.position;
        Vector2 size = spawnArea.localScale;

        // Posisi random
        Vector2 randomPos = new Vector2(
            Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            Random.Range(center.y - size.y / 2, center.y + size.y / 2)
        );

        // Spawn enemy
        Instantiate(enemyPrefab, randomPos, Quaternion.identity);
    }

    // Visual area spawn
    void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                spawnArea.position,
                spawnArea.localScale
            );
        }
    }
}