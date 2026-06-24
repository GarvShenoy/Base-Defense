using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private int enemyHealth = 1;
    [SerializeField] private int enemyDefense = 0;

        public void ReceiveDamage(int damage)
        {
            int newDamage = damage - enemyDefense;

            if (damage > 0)
            {
                int newHealth = enemyHealth - newDamage;
                enemyHealth = newHealth;
            }
        }

        private void Update()
        {
            if (enemyHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
}
