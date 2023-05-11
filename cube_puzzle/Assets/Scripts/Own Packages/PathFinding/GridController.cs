using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridController : MonoBehaviour
{
    [SerializeField]
    private int levelHeight;
    public int YMax { get { return (int)(levelHeight / 2 / grid.cellSize.y); } }
    public int YMin { get { return (int)(-levelHeight / 2 / grid.cellSize.y); } }

    [SerializeField]
    private int levelWidth;
    public int XMax { get { return (int)(levelWidth / 2 / grid.cellSize.x); } }
    public int XMin { get { return (int)(-levelWidth / 2 / grid.cellSize.x); } }

    public Grid grid;

    [SerializeField]
    private GameObject testObject;

    [SerializeField]
    private bool drawTestObjects = false;

    private GameObject collisionChecker;
    private BoxCollider2D collisionCheckerCollider;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void CreateCollisionChecker()
    {
        collisionChecker = Instantiate(new GameObject(name = "Collision Checker"), transform);

        collisionCheckerCollider = collisionChecker.AddComponent<BoxCollider2D>();
        collisionCheckerCollider.size = new Vector2(grid.cellSize.x, grid.cellSize.y);
    }

    public Vector2 TileToWorldPosition(Vector2Int tilePosition)
    {
        return new Vector2(tilePosition.x * grid.cellSize.x, tilePosition.y * grid.cellSize.y);
    }

    public Vector2Int WorldToTilePosition(Vector2 worldPosition)
    {
        return new Vector2Int((int)(worldPosition.x / grid.cellSize.x), (int)(worldPosition.y / grid.cellSize.y));
    }

    public void InstantiateTestObject(Vector2 worldPosition)
    {
        if (drawTestObjects)
            Instantiate(testObject, worldPosition, Quaternion.identity);
    }

    public Collider2D[] GetCollisionsAt(Vector2 position, LayerMask layerMask)
    {
        collisionChecker.transform.position = position;

        int numColliders = 20;
        Collider2D[] colliders = new Collider2D[numColliders];
        ContactFilter2D contactFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = layerMask
        };

        collisionCheckerCollider.OverlapCollider(contactFilter, colliders);

        return colliders;
    }
}