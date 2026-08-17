using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    [Header("Placement")]
    [SerializeField] private LayerMask placementLayer;

    private GameObject previewTower;

    //It checks if TowerPlaceManager exists and destroys any active preview tower if no tower is currently selected.
    //It then converts the current mouse position to world coordinates and instantiates a semi-transparent preview tower if one does not exist.
    //It then updates the preview tower's position to follow the mouse cursor and adjusts its tint color based on placement validity.
    //Lastly it handles left-clicks to place the tower on valid terrain when not clicking on UI, and right-clicks to cancel placement.
    private void Update()
    {
        if (TowerPlaceManager.main == null)
            return;

        if (TowerPlaceManager.main.SelectedTowerData == null)
        {
            if (previewTower != null)
            {
                Destroy(previewTower);
                previewTower = null;
            }

            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        if (previewTower == null)
        {
            GameObject previewPrefab =
                TowerPlaceManager.main.SelectedTowerData.tempTowerPrefab;

            if (previewPrefab == null)
            {
                Debug.LogWarning(
                    "No Temp Tower Prefab assigned to " +
                    TowerPlaceManager.main.SelectedTowerData.towerName
                );

                return;
            }

            previewTower = Instantiate(previewPrefab);

            SpriteRenderer[] renderers =
                previewTower.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sr in renderers)
            {
                Color color = sr.color;
                color.a = 0.5f;
                sr.color = color;
            }
        }

        previewTower.transform.position = mousePos;

        bool validPlacement = IsValidPlacement(mousePos);

        SetPreviewColor(validPlacement);

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            if (validPlacement)
            {
                PlaceTower(mousePos);
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
        }
    }

    //It checks if a 2D collider exists at the target position on the designated placement layer.
    //It returns true if a valid placement area is detected under the cursor.
    private bool IsValidPlacement(Vector2 position)
    {
        Collider2D placementCollider = Physics2D.OverlapPoint(
            position,
            placementLayer
        );

        return placementCollider != null;
    }

    //It checks if the preview tower exists and retrieves all of its child SpriteRenderer components.
    //It sets the sprite renderers' tint to semi-transparent green if placement is valid, or semi-transparent red if invalid.
    private void SetPreviewColor(bool valid)
    {
        if (previewTower == null)
            return;

        SpriteRenderer[] renderers =
            previewTower.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            Color color = valid ? Color.green : Color.red;
            color.a = 0.5f;

            sr.color = color;
        }
    }

    //It retrieves the selected tower data and instantiates the permanent tower prefab at the target position.
    //It destroys the preview tower instance, registers the placed tower, deducts its cost from player funds, and clears the current selection.
    private void PlaceTower(Vector2 position)
    {
        TowerData towerData =
            TowerPlaceManager.main.SelectedTowerData;

        if (towerData == null)
            return;
            
        Instantiate(
            towerData.towerPrefab,
            position,
            Quaternion.identity
        );

        Destroy(previewTower);
        previewTower = null;

        TowerPlaceManager.main.RegisterTower(towerData);
        MoneyManager.main.SpendMoney(towerData.towerCost);
        TowerPlaceManager.main.ClearSelection();
    }

    //It destroys the active preview tower instance if it exists and resets the preview reference.
    //It then calls ClearSelection on TowerPlaceManager to cancel the tower placement process.
    private void CancelPlacement()
    {
        if (previewTower != null)
        {
            Destroy(previewTower);
            previewTower = null;
        }

        TowerPlaceManager.main.ClearSelection();
    }
}