using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float baseTimeBetweenEnemiesSpawn = 3f;
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform GameField;

    private float timeBetweenEnemiesSpawn = 3f;
    private List<Enemy> enemies = new List<Enemy>();
    private float timeSinceLastEnemy = 0f;
    private int totalEnemiesCoint = 0;
    private Vector3[] worldCorners = new Vector3[4];
    private const float DIFFICULTY_MULTIPLIER = 0.5f;

    void Start()
    {
        GameField.GetWorldCorners(worldCorners);
        SpawnEnemy();
    }

    void Update()
    {
        //—легка повышаем сложность через каждые 5 врагов до определенного предела
        timeBetweenEnemiesSpawn = baseTimeBetweenEnemiesSpawn - ((int)(totalEnemiesCoint/5) * DIFFICULTY_MULTIPLIER);
        timeBetweenEnemiesSpawn = Mathf.Max(timeBetweenEnemiesSpawn, 1);

        Debug.Log(timeBetweenEnemiesSpawn);

        timeSinceLastEnemy += Time.deltaTime;
        if (timeSinceLastEnemy >= timeBetweenEnemiesSpawn && !GameManager.Instance.isGameOver)
        {
            SpawnEnemy();
            timeSinceLastEnemy = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomEnemyPosition();
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        enemy.SetTarget(playerTransform.position);
        enemy.SetSpeed(enemySpeed);
        enemies.Add(enemy);
        totalEnemiesCoint++;
    }

    private Vector2 GetRandomEnemyPosition()
    {
        //¬ыбираем случайную угловую точку, и вторую - смежную с ней
        int firstCornerIndex = Random.Range(0, 4);
        int secondCornerIndex;
        if (Random.value < 0.5f)
            secondCornerIndex = firstCornerIndex - 1;
        else
            secondCornerIndex = firstCornerIndex + 1;
        if (secondCornerIndex == 4) secondCornerIndex = 0;
        if (secondCornerIndex == -1) secondCornerIndex = 3;

        //Ќаходим случайную точку на получившемс€ отрезке
        Vector2 position = worldCorners[firstCornerIndex] + Random.Range(0f, 1f) * (worldCorners[secondCornerIndex] - worldCorners[firstCornerIndex]);
        return position;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public List<Enemy> GetEnemies()
    {
        return enemies;
    }
}