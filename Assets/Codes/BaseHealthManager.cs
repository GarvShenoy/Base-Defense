using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseHealthManager : MonoBehaviour
{
    public static BaseHealthManager main;

    [SerializeField] private int startHealth = 25;
    [SerializeField] private string sceneName;

    private int currentHealth;
    
    private void Awake()
    {
        main = this;
    }

    //Sets the Current Health to max health at the start of the level.
    private void Start()
    {
        currentHealth = startHealth;
    }

    public int GetHealth()
    {
        return currentHealth;
    }
    
    public int GetMaxHealth()
    {
        return startHealth;
    }

    //Called by other codes to deal damage to the base. Health lost will be subtracted from the current health and updates the current Health.
    //If the base health reaches 0 or less than 0, the code loads a named scene, which in this case is LoadScene.
    //Otherwise, it debugs the current health. 
    public void LoseHealth(int lostHealth)
        {
            currentHealth -= lostHealth;
            if (currentHealth <= 0)
            {
                SceneManager.LoadScene(sceneName);
            }
            Debug.Log("Lost Health. Current Health: " + currentHealth);
        }
}

