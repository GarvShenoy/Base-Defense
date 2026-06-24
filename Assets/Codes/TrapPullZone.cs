using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPullZone : MonoBehaviour
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

        Transform baseTarget = LevelManager.main.endPoint;
        Transform trapCenter = transform.parent;

        float distToBase = Vector2.Distance(other.transform.position, baseTarget.position);
        float distToTrap = Vector2.Distance(other.transform.position, trapCenter.position);

        if (distToTrap < distToBase)
        {
            enemy.SetTarget(trapCenter);
        }
    }
}
