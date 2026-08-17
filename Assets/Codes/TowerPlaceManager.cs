using System.Collections.Generic;
using UnityEngine;

public class TowerPlaceManager : MonoBehaviour
{
    public static TowerPlaceManager main;

    public TowerData SelectedTowerData { get; private set; }

    private List<TowerData> placedTowers = new List<TowerData>();

    private void Awake()
    {
        main = this;
    }

    //It first checks if the tower data or MoneyManager is missing and exits if either is null.
    //It then verifies if the player has enough currency for the tower, logging a message and stopping if funds are insufficient.
    //It then checks if the placement limit for this tower type has been reached.
    //If all checks pass, it sets SelectedTowerData to the chosen tower.
    public void SelectTower(TowerData towerData)
    {
        if (towerData == null)
            return;

        if (MoneyManager.main == null)
            return;

        if (MoneyManager.main.GetMoney() < towerData.towerCost)
        {
            Debug.Log("Not Enough Money");
            return;
        }

        if (GetTowerCount(towerData) >= towerData.towerLimit)
        {
            Debug.Log("Tower limit reached");
            return;
        }

        SelectedTowerData = towerData;
    }

    //It checks if the provided tower data is valid and adds it to the placedTowers list.
    public void RegisterTower(TowerData towerData)
    {
        if (towerData == null)
            return;

        placedTowers.Add(towerData);
    }

    //It loops through the placedTowers list to count how many match the given tower data.
    //It then returns the total count of placed instances for that specific tower.
    public int GetTowerCount(TowerData towerData)
    {
        int count = 0;

        foreach (TowerData placedTower in placedTowers)
        {
            if (placedTower == towerData)
            {
                count++;
            }
        }

        return count;
    }

    //It resets SelectedTowerData to null to clear the current tower selection.
    public void ClearSelection()
    {
        SelectedTowerData = null;
    }
}