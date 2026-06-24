using UnityEngine;

public class TowerGun : TowerBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    protected override void ExecuteAttack(EnemyHealth target)
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