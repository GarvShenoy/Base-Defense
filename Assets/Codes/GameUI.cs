using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthBar;

    private void Update()
    {
        UpdateMoney();
        UpdateWave();
        UpdateHealth();
    }

    //This updates the Money Text UI in GameUI every frame for any changes that have occured to it.
    private void UpdateMoney()
    {
        if (MoneyManager.main == null || moneyText == null)
            return;

        moneyText.text =
            "$" + MoneyManager.main.GetMoney();
    }

    //This updates the Wave Text UI in GameUI every frame for any changes that have occured to it.
    private void UpdateWave()
    {
        if (EnemySpawner.main == null || waveText == null)
            return;

        waveText.text =
            "Wave " +
            EnemySpawner.main.GetCurrentWave();
    }

    //It grabs both health values, before setting it into the slider UI reused as a health bar.
    //This also updates the Health Text UI in GameUI every frame for any changes that have occured to it.
    private void UpdateHealth()
    {
        if (BaseHealthManager.main == null)
            return;

        int currentHealth =
            BaseHealthManager.main.GetHealth();

        int maxHealth =
            BaseHealthManager.main.GetMaxHealth();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text =
                currentHealth + " / " + maxHealth;
        }
    }
}