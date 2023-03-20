using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _enemyHitVFX;
    [SerializeField] private GameObject _projectilePrefab;

    private float timeSinceLastShot = 0f;
    private EnemySpawner enemySpawner;
    private List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemies = enemySpawner.GetEnemies();
    }

    void Update()
    {
        var gameSettings = GameManager.Instance.GameSettings;
        float FireRate = gameSettings.FireRateValues[gameSettings.FireRateLevel - 1];
        float ShootingDistance = gameSettings.ShootingDistanceValues[gameSettings.ShootingDistanceLevel - 1];

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot < FireRate) return;

        var closestEnemy = FindClosestEnemy(ShootingDistance);
        if (closestEnemy != null)
        {
            if (closestEnemy.LifesLeft <= 1 )
            {
                closestEnemy.EnableNoMoreShotsNeeded();
            }
            Shoot(closestEnemy.transform.position);
        }

        timeSinceLastShot = 0f;
    }

    private Enemy FindClosestEnemy(float ShootingDistance)
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.NoMoreShotsNeeded) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance < ShootingDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void Shoot(Vector2 targetPosition)
    {
        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetTarget(targetPosition);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject vfx = Instantiate(_enemyHitVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f);

            enemySpawner.RemoveEnemy(other.gameObject.GetComponent<Enemy>());
            Destroy(other.gameObject);

            GameManager.Instance.HandleGameOver();
            Destroy(gameObject);
        }
    }
}