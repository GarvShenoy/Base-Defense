using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSlowZone : MonoBehaviour
{
    private Trap parentTrap;

    private void Start()
    {
        parentTrap = GetComponentInParent<Trap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyMovement enemy = other.GetComponent<EnemyMovement>();
        if (enemy == null) return;

        enemy.ApplySlow(parentTrap.GetSlowMultiplier());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyMovement enemy = other.GetComponent<EnemyMovement>();
        if (enemy == null) return;

        enemy.RemoveSlow();
    }
}