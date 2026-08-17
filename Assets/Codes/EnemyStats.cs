using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int enemyHealth = 1;
    [SerializeField] private int enemyDefense = 0;
    [SerializeField] private int enemyMoney = 10;

    private int currentEnemyHealth;

    public int CurrentEnemyHealth => currentEnemyHealth;

    //Sets the Current Enemy Health to set enemy health whenever it is instantiated.
    private void Awake()
    {
        currentEnemyHealth = enemyHealth;
    }

    //When this method is called, it first removes the original damage by the defense of the mutant.
    //It then updates the current health using the new calculated damage, if it is above 0.
    //In the case where enemy health is <= 0, it calls Die().
    public void ReceiveDamage(int damage)
    {
        int newDamage = damage - enemyDefense;
        
        if (newDamage > 0)
        {
            currentEnemyHealth -= newDamage;
        }

        if (currentEnemyHealth <= 0)
        {
            Die();
        }
    }

    //Calls the method in Money Manager to add the money earned for defeating the mutant.
    //It then destroys the mutant game object.
    private void Die()
    {
         MoneyManager.main.RecieveMoney(enemyMoney);
        Destroy(gameObject);
    }
}