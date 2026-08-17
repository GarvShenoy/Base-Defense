using UnityEngine;

public class TowerGun : TowerBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    //It first checks if the target, projectile prefab, or fire point is missing and stops execution if any are invalid.
    //It then calculates the normalized direction vector pointing from the fire point to the target enemy.
    //Next, it spawns the projectile prefab at the fire point's position and retrieves its Projectile component.
    //If the script is found, it initializes the projectile with the target direction and attack damage.
    protected override void ExecuteAttack(EnemyStats target)
    {
        if (!target) return;

        if (projectilePrefab == null || firePoint == null) return;

        Vector2 direction =
            (target.transform.position - firePoint.position).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectileScript = proj.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.Initialize(direction, damage);
        }
    }
}