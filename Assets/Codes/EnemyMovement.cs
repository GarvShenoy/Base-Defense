using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour 
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")] 
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;
    

    private Transform target;

    private float originalSpeed;
    private int slowSources = 0;

    private void Start()
    {
        target = LevelManager.main.endPoint;
        originalSpeed = moveSpeed;
    }

    private void Update()
    {
        if (Vector2.Distance(LevelManager.main.endPoint.position, transform.position) <= 0.1f)
        {
            Destroy(gameObject);
        }

        if (target != LevelManager.main.endPoint &&
            Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            target = LevelManager.main.endPoint;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        if (direction != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle - 90f);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void ApplySlow(float multiplier)
    {
        slowSources++;
        moveSpeed = originalSpeed * multiplier;
    }

    public void RemoveSlow()
    {
        slowSources--;
        if (slowSources <= 0)
        {
            slowSources = 0;
            moveSpeed = originalSpeed;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
