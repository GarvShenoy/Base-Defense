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

    [Header("Rotation")]
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float rotationOffset = -90f;
    [SerializeField] protected float fireAngleThreshold = 5f;

    protected bool isAttacking = false;

    protected CircleCollider2D rangeCollider;
    protected List<EnemyHealth> enemiesInRange = new List<EnemyHealth>();

    protected virtual void Awake()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
    }

    protected virtual void Start()
    {
        UpdateRange();
    }

    protected virtual void OnValidate()
    {
        UpdateRange();
    }

    protected void UpdateRange()
    {
        if (rangeCollider != null)
        {
            rangeCollider.radius = towerRange;
        }
    }

    protected virtual void Update()
    {
        enemiesInRange.RemoveAll(e => !e);

        EnemyHealth target = GetClosestToBaseEnemy();

        if (target)
        {
            RotateTowardsTarget(target);
        }

        if (!isAttacking && target)
        {
            StartCoroutine(AttackCycle());
        }
    }

    private IEnumerator AttackCycle()
    {
        isAttacking = true;

        for (int i = 0; i < attacksPerCycle; i++)
        {
            EnemyHealth target = GetClosestToBaseEnemy();

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

    protected abstract void ExecuteAttack(EnemyHealth target);

    protected void RotateTowardsTarget(EnemyHealth target)
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

    protected bool IsFacingTarget(EnemyHealth target)
    {
        if (!target) return false;

        Vector2 direction = (target.transform.position - transform.position).normalized;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;

        float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);

        return Mathf.Abs(angleDifference) <= fireAngleThreshold;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    protected EnemyHealth GetClosestToBaseEnemy()
    {
        EnemyHealth closest = null;
        float closestDistance = Mathf.Infinity;

        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            EnemyHealth enemy = enemiesInRange[i];

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