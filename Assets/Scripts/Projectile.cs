using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 45f;
    private Vector3 targetPos;
    private const float SPEED_MULTIPLIER = 0.1f;
    
    void Update()
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.position += direction * speed * SPEED_MULTIPLIER * Time.deltaTime;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }
}
