using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] private TowerData towerData;

    [Header("UI")]
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text towerCostText;
    [SerializeField] private TMP_Text towerCountText;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        UpdateUI();
    }

    //Ensures these methods are called every frame.
    private void Update()
    {
        UpdateButtonState();
        UpdateTowerCount();
    }

    //This sets the values in the UI at the start of the level.
    private void UpdateUI()
    {
        if (towerData == null)
            return;

        if (towerNameText != null)
            towerNameText.text = towerData.towerName;

        if (towerCostText != null)
            towerCostText.text = "$" + towerData.towerCost;

        UpdateTowerCount();
    }

    //This updates the tower count every frame in case of change.
    private void UpdateTowerCount()
    {
        if (towerData == null || towerCountText == null)
            return;

        int towersPlaced = 0;

        if (TowerPlaceManager.main != null)
        {
            towersPlaced = TowerPlaceManager.main.GetTowerCount(towerData);
        }

        towerCountText.text = towersPlaced + " / " + towerData.towerLimit;
    }

    //This updates whether the button can be pressed at any point in time in case the player does not have enough money or has reached the max tower limit.
    private void UpdateButtonState()
    {
        if (button == null || towerData == null)
            return;

        bool enoughMoney = MoneyManager.main != null && MoneyManager.main.GetMoney() >= towerData.towerCost;
        bool underLimit = TowerPlaceManager.main != null && TowerPlaceManager.main.GetTowerCount(towerData) < towerData.towerLimit;
        button.interactable = enoughMoney && underLimit;
    }

    //It checks if towerData is null and exits the method if no tower data is assigned.
    //It then passes towerData to the TowerPlaceManager to set the selected tower for placement.
    public void SelectTower()
    {
        if (towerData == null)
            return;

        TowerPlaceManager.main.SelectTower(towerData);
    }
}