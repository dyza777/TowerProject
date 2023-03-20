using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 5f;
    private Vector3 targetPos;
    private EnemySpawner enemySpawner;

    [SerializeField] private GameObject _playerHitVFX;
    [SerializeField] private int coinsReward = 100;
    public bool NoMoreShotsNeeded { get; private set; } = false;
    public int LifesLeft { get; private set; } = 3;
    
    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }

    void Update()
    {
        var gameSettings = GameManager.Instance.GameSettings;
        int enemiesLifesCount = (int)gameSettings.ShotsToDestroyValues[gameSettings.ShotsToDestroyLevel - 1];
        if (LifesLeft > enemiesLifesCount)
        {
            LifesLeft = enemiesLifesCount;
        }

        if (GameManager.Instance.isGameOver)
        {
            transform.Rotate(new Vector3(0,0, 30 * Time.deltaTime));
            return;
        }

        Vector3 direction = (targetPos - transform.position).normalized;
        transform.position += direction * _speed * 0.1f * Time.deltaTime;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            GameObject vfx = Instantiate(_playerHitVFX, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(vfx, 2f);

            if (LifesLeft > 1)
            {
                LifesLeft -= 1;
            } else
            {
                enemySpawner.RemoveEnemy(this);
                CoinsManager.Instance.AddCoins(coinsReward);
                Destroy(gameObject);
            }
        }
    }

    public void EnableNoMoreShotsNeeded()
    {
        NoMoreShotsNeeded = true;
    }

    public void SetSpeed (float value)
    {
        _speed = value;
    }
}