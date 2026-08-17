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

    //It first initializes the projectile's normalized movement direction and damage value.
    //It then calls RotateProjectile() to orient the sprite correctly and schedules the object's destruction after projectileLifetime, which is it's time limit.
    public void Initialize(Vector2 direction, int projectileDamage)
    {
        moveDirection = direction.normalized;
        damage = projectileDamage;

        RotateProjectile();

        Destroy(gameObject, projectileLifetime);
    }

    //This method translates the projectile's position frame-by-frame along moveDirection, scaled by projectileSpeed and Time.deltaTime.
    private void Update()
    {
        transform.position += (Vector3)(moveDirection * projectileSpeed * Time.deltaTime);
    }

    //Once it detects 2D collisions, it checks if the hit object has an EnemyStats component.
    //If it is indeed an enemy, it applies the damage via ReceiveDamage() and destroys the projectile.
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats enemy = other.GetComponent<EnemyStats>();

        if (enemy)
        {
            enemy.ReceiveDamage(damage);

            Destroy(gameObject);
        }
    }

    //This calculates the angle of travel using moveDirection and converts it to degrees.
    //It then applies a 2D Z-axis rotation to face the projectile toward its moving direction with a -90 degree offset, as the prefab is not facing the right direction.
    private void RotateProjectile()
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}