using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;

    public void SelectTower()
    {   
        Debug.Log("Tower Selected");
        TowerPlaceManager.main.SelectTower(towerPrefab);
    }
}