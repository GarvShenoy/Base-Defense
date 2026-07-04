using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    [Header("Placement")]
    [SerializeField] private LayerMask placementLayer;

    private GameObject previewTower;

    private void Update()
    {
        if (TowerPlaceManager.main.SelectedTowerPrefab == null)
        {
            if (previewTower != null)
            {
                Destroy(previewTower);
                previewTower = null;
            }

            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (previewTower == null)
        {
            previewTower = Instantiate(TowerPlaceManager.main.SelectedTowerPrefab);

            SpriteRenderer sr = previewTower.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 0.5f;
                sr.color = c;
            }
        }
        previewTower.transform.position = mousePos;

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceTower(mousePos);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
        }
    }

    private void PlaceTower(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapPoint(position, placementLayer);

        if (hit == null)
            return;

        Instantiate(TowerPlaceManager.main.SelectedTowerPrefab, position, Quaternion.identity);

        Destroy(previewTower);
        previewTower = null;

        TowerPlaceManager.main.ClearSelection();
    }

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