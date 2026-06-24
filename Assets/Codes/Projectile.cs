using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 2f;

    private Vector2 moveDirection;
    private int damage;

    public void Initialize(Vector2 direction, int projectileDamage)
    {
        moveDirection = direction.normalized;
        damage = projectileDamage;

        RotateProjectile();

        Destroy(gameObject, projectileLifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDirection * projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy)
        {
            enemy.ReceiveDamage(damage);

            Destroy(gameObject);
        }
    }

    private void RotateProjectile()
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}