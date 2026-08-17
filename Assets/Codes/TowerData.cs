using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    //Stores all tower data for easy access.
    [Header("Tower Information")]
    public string towerName;

    [Header("Tower Settings")]
    public int towerCost;
    public int towerLimit;

    [Header("Prefabs")]
    public GameObject towerPrefab;
    public GameObject tempTowerPrefab;
}