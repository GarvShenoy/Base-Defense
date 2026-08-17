using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private EnemyStats enemyStats;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Transform target;

    private void Awake()
    {
        if (enemyStats == null)
        {
            enemyStats = GetComponent<EnemyStats>();
        }
    }

    //Sets the end goal of the mutant as the endpoint in level manager. The enemy will use this to head there.
    private void Start()
    {
        target = LevelManager.main.endPoint;
    }

    //The code first ensures that there is a level manager.
    //Then it checks if the distance between the mutant's position and the end goal position is below 0.1f.
    //If it is, it destroys itself and removes health from the Base.
    private void Update()
    {
        if (LevelManager.main == null)
            return;

        if (Vector2.Distance(LevelManager.main.endPoint.position,transform.position) <= 0.1f)
        {
            if (BaseHealthManager.main != null && enemyStats != null)
            {
                BaseHealthManager.main.LoseHealth(enemyStats.CurrentEnemyHealth );
            }

            Destroy(gameObject);
        }
    }

    //This method also checks for a valid target before calculating a normalized direction vector pointing towards the end goal.
    //It then sets the Rigidbody2D linear velocity to move the object toward the target at moveSpeed.
    //Lastly, it calculates the target angle using Quaternion and smoothly rotates the transform toward the target direction using Quaternion.Lerp.
    private void FixedUpdate()
    {
        if (target == null)
            return;

        Vector2 direction =
            (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        if (direction != Vector2.zero)
        {
            float targetAngle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    targetAngle - 90f
                );

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}