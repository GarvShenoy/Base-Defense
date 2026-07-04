using UnityEngine;

public class TowerPlaceManager : MonoBehaviour
{
    public static TowerPlaceManager main;

    private void Awake()
    {
        main = this;
    }

    public GameObject SelectedTowerPrefab { get; private set; }

    public void SelectTower(GameObject towerPrefab)
    {
        SelectedTowerPrefab = towerPrefab;
    }

    public void ClearSelection()
    {
        SelectedTowerPrefab = null;
    }
}
