using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CircleCollider2D outerSpawnZone;
    [SerializeField] private CircleCollider2D innerNoSpawnZone;

    [Header("Enemy Types")]
    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject groupEnemyPrefab;
    [SerializeField] private GameObject tankEnemyPrefab;


    [Header("Enemy Types")]
    [SerializeField] private float groupEnemyChance = 0.2f;
    [SerializeField] private float tankEnemyChance = 0.4f;


    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave = 20;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float timeBetweenWaves = 5f;

    [Header("Group Settings")]
    [SerializeField] private float groupSpreadRadius = 0.3f;
    [SerializeField] private float addEnemyChance = 0.05f;

    [Header("Spawn Spacing")]
    [SerializeField] private float minDistanceBetweenSpawns = 5f;

    private int enemiesRemaining;
    private int currentWave = 0;

    private Vector2 lastSpawnPos;
    private Vector2 secondLastSpawnPos;
    private bool hasLast = false;
    private bool hasSecondLast = false;

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            currentWave++;
            enemiesRemaining = enemiesPerWave;

            hasLast = false;
            hasSecondLast = false;

            Debug.Log("Wave " + currentWave + " started!");

            yield return SpawnWave();

            Debug.Log("Wave " + currentWave + " ended!");

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnWave()
    {
        while (enemiesRemaining > 0)
        {
            SpawnNext();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNext()
    { 
        if (Random.value < groupEnemyChance)
            SpawnSpecialGroup();
        else
            if (Random.value < tankEnemyChance)
                SpawnSingleEnemy(tankEnemyPrefab);
            else
                SpawnSingleEnemy(normalEnemyPrefab);
    }

    private void SpawnSingleEnemy(GameObject enemyChosen)
    {
        Vector2 pos = GetValidSpawnPosition();

        Instantiate(enemyChosen, pos, Quaternion.identity);

        enemiesRemaining--;
        UpdateSpawnHistory(pos);
    }

    private void SpawnSpecialGroup()
    {
        int groupSize = 3;

        while (Random.value < addEnemyChance)
        {
            groupSize++;
        }

        Vector2 center = GetValidSpawnPosition();

        for (int i = 0; i < groupSize; i++)
        {
            Vector2 offset = Random.insideUnitCircle * groupSpreadRadius;
            Instantiate(groupEnemyPrefab, center + offset, Quaternion.identity);
        }

        enemiesRemaining--;
        UpdateSpawnHistory(center);
    }

    private Vector2 GetValidSpawnPosition()
    {
        Vector2 center = outerSpawnZone.transform.position;

        float outerRadius = outerSpawnZone.radius * outerSpawnZone.transform.lossyScale.x;
        float innerRadius = innerNoSpawnZone.radius * innerNoSpawnZone.transform.lossyScale.x;

        int attempts = 0;

        while (attempts < 50)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            float dist = Random.Range(innerRadius, outerRadius);
            Vector2 pos = center + dir * dist;

            bool tooCloseToLast = hasLast &&
                Vector2.Distance(pos, lastSpawnPos) < minDistanceBetweenSpawns;

            bool tooCloseToSecond = hasSecondLast &&
                Vector2.Distance(pos, secondLastSpawnPos) < minDistanceBetweenSpawns;

            if (!tooCloseToLast && !tooCloseToSecond)
                return pos;

            attempts++;
        }

        return center;
    }

    private void UpdateSpawnHistory(Vector2 pos)
    {
        secondLastSpawnPos = lastSpawnPos;
        hasSecondLast = hasLast;

        lastSpawnPos = pos;
        hasLast = true;
    }
}