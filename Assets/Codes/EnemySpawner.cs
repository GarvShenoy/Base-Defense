using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner main;
    
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
    [SerializeField] private int moneyPerWave = 100;

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

    private void Awake()
    {
        main = this;
    }

    //It starts the WaveLoop coroutine when the script initializes.
    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    //It runs an infinite loop that increments the wave count, resets spawn parameters, triggers enemy spawning, awards money via MoneyManager, and waits for timeBetweenWaves before starting the next wave.
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
            MoneyManager.main.RecieveMoney(moneyPerWave);

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    //It returns the current wave number.
    public int GetCurrentWave()
    {
        return currentWave;
    }

    //It continuously spawns enemies and waits for spawnInterval after each spawn until no remaining enemies are left for the wave.
    private IEnumerator SpawnWave()
    {
        while (enemiesRemaining > 0)
        {
            SpawnNext();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //It evaluates random probability checks to decide whether to spawn a special group, a tank enemy, or a standard enemy.
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

    //It gets a valid spawn location, instantiates the chosen enemy prefab, decrements the wave's remaining enemy count, and updates spawn position history.
    private void SpawnSingleEnemy(GameObject enemyChosen)
    {
        Vector2 pos = GetValidSpawnPosition();

        Instantiate(enemyChosen, pos, Quaternion.identity);

        enemiesRemaining--;
        UpdateSpawnHistory(pos);
    }

    //It determines a random group size, selects a center spawn position, and instantiates multiple group enemy prefabs scattered within a spread radius before updating remaining count and spawn history.
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

    //It calculates an annular spawn ring between the inner and outer zone radii and attempts up to 50 times to find a position that maintains minDistanceBetweenSpawns from recent spawn locations.
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

            bool tooCloseToLast = hasLast && Vector2.Distance(pos, lastSpawnPos) < minDistanceBetweenSpawns;

            bool tooCloseToSecond = hasSecondLast && Vector2.Distance(pos, secondLastSpawnPos) < minDistanceBetweenSpawns;

            if (!tooCloseToLast && !tooCloseToSecond)
                return pos;

            attempts++;
        }

        return center;
    }

    //It shifts the current last spawn position to secondLastSpawnPos and records the newly provided position as lastSpawnPos.
    private void UpdateSpawnHistory(Vector2 pos)
    {
        secondLastSpawnPos = lastSpawnPos;
        hasSecondLast = hasLast;

        lastSpawnPos = pos;
        hasLast = true;
    }
}