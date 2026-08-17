using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Common Attributes")]
    [SerializeField] protected float towerCooldown = 2f;
    [SerializeField] protected int attacksPerCycle = 1;
    [SerializeField] protected float attackDelay = 1f;
    [SerializeField] protected float towerRange = 2f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected int cost = 100;


    [Header("Rotation")]
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float rotationOffset = -90f;
    [SerializeField] protected float fireAngleThreshold = 5f;

    protected bool isAttacking = false;

    protected CircleCollider2D rangeCollider;
    protected List<EnemyStats> enemiesInRange = new List<EnemyStats>();

    //It retrieves the CircleCollider2D component attached to this GameObject during initialization and assigns it to rangeCollider.
    protected virtual void Awake()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
    }

    //It calls UpdateRange() to initialize the tower's collision radius when the script starts.
    protected virtual void Start()
    {
        UpdateRange();
    }

    //It calls UpdateRange() to update the range collider radius whenever values are modified in the Unity Inspector.
    protected virtual void OnValidate()
    {
        UpdateRange();
    }

    //It checks if rangeCollider is assigned and sets its radius to match towerRange.
    protected void UpdateRange()
    {
        if (rangeCollider != null)
        {
            rangeCollider.radius = towerRange;
        }
    }

    //It removes null enemy references from the range list and identifies the enemy closest to the base.
    //It then rotates toward the target enemy and starts the AttackCycle coroutine if the tower is not already attacking.
    protected virtual void Update()
    {
        enemiesInRange.RemoveAll(e => !e);

        EnemyStats target = GetClosestToBaseEnemy();

        if (target)
        {
            RotateTowardsTarget(target);
        }

        if (!isAttacking && target)
        {
            StartCoroutine(AttackCycle());
        }
    }

    //It sets the attacking state and loops through the assigned number of attacks per cycle.
    //It checks if the target is in range, waits until the tower is facing the enemy (up to maxAimTime), triggers ExecuteAttack(), and applies attack delays and cooldowns before resetting the state.
    private IEnumerator AttackCycle()
    {
        isAttacking = true;

        for (int i = 0; i < attacksPerCycle; i++)
        {
            EnemyStats target = GetClosestToBaseEnemy();

            if (target && enemiesInRange.Contains(target))
            {
                float aimTimer = 0f;
                float maxAimTime = 1f;

                while (!IsFacingTarget(target))
                {
                    if (!target || !enemiesInRange.Contains(target))
                        break;

                    aimTimer += Time.deltaTime;

                    if (aimTimer >= maxAimTime)
                        break;

                    yield return null;
                }

                if (target)
                {
                    ExecuteAttack(target);
                }
            }

            yield return new WaitForSeconds(attackDelay);
        }

        yield return new WaitForSeconds(towerCooldown);

        isAttacking = false;
    }

    //Abstract method meant to be overridden by child tower classes to perform specific attack logic against the target enemy.
    protected abstract void ExecuteAttack(EnemyStats target);

    //It calculates the angle toward the target enemy using their relative positions.
    //It then smoothly rotates the tower toward that target angle using Quaternion.Lerp.
    protected void RotateTowardsTarget(EnemyStats target)
    {
        if (!target) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;

        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    //It calculates the angular difference between the tower's current rotation and the target enemy's direction.
    //It then returns true if the angle difference is within the permitted fireAngleThreshold.
    protected bool IsFacingTarget(EnemyStats target)
    {
        if (!target) return false;

        Vector2 direction = (target.transform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);

        return Mathf.Abs(angleDifference) <= fireAngleThreshold;
    }

    //It detects when an object enters the trigger area and adds its EnemyStats component to the enemiesInRange list if it is not already listed.
    protected void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats enemy = other.GetComponent<EnemyStats>();
        if (enemy && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    //It detects when an object exits the trigger area and removes its EnemyStats component from the enemiesInRange list.
    protected void OnTriggerExit2D(Collider2D other)
    {
        EnemyStats enemy = other.GetComponent<EnemyStats>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    //It iterates backward through enemiesInRange, removing destroyed null targets while calculating each enemy's distance to the end point.
    //It then returns the EnemyStats instance that is closest to the level's destination point.
    protected EnemyStats GetClosestToBaseEnemy()
    {
        EnemyStats closest = null;
        float closestDistance = Mathf.Infinity;

        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            EnemyStats enemy = enemiesInRange[i];

            if (!enemy)
            {
                enemiesInRange.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(
                enemy.transform.position,
                LevelManager.main.endPoint.position
            );

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = enemy;
            }
        }

        return closest;
    }
}