using UnityEngine;

public class ConfinementSize : MonoBehaviour
{
    private void Start()
    {
        CustomGrid grid = FindObjectOfType<CustomGrid>();
        Tile tile = FindObjectOfType<Tile>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float size = tile.transform.localScale.x * grid.Width;
        collider.size = new Vector2(size, size);
    }
}
