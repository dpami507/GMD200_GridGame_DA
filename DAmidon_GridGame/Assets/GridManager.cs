using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int numRows;
    public int numColumns;

    public float padding;
    public float squareSize;

    public Vector2 gridOffset;
    public Transform tilesParent;

    [SerializeField] GridTile tilePrefab;
    public List<GridTile> tiles;

    public GameObject playerPrefab;
    [HideInInspector] public PlayerScript player;

    public static GridManager instance;

    private void Awake()
    {
        instance = this;

        InitGrid();
        CreatePlayer();
    }

    void InitGrid()
    {
        //Create Tiles
        for (int i = 0; i < numRows; i++)
        {
            for(int j = 0; j < numColumns; j++)
            {
                GridTile tile = Instantiate(tilePrefab, tilesParent);
                tiles.Add(tile);

                tile.transform.localScale = Vector3.one * squareSize;
                Vector2 tilePos = new Vector2((j * squareSize) + (padding * j), (i * squareSize) + (padding * i));
                tile.transform.localPosition = tilePos;
                tile.name = $"Tile ({j}, {i})";
                tile.gridManager = this;
                tile.gridChords = new Vector2Int(j, i);
            }
        }

        //Center Grid
        float y = -(((numRows - 1) * squareSize) + ((numRows - 1) * padding)) / 2f;
        float x = -(((numColumns - 1) * squareSize) + ((numColumns - 1) * padding)) / 2f;
        gridOffset = new Vector2(x, y);
        transform.position = gridOffset;
    }

    void CreatePlayer()
    {
        player = Instantiate(playerPrefab, transform).GetComponent<PlayerScript>();
        player.transform.localScale = Vector3.one * squareSize;
        player.gridManager = this;
        /*player.gridChords =*/ ClampPoint(Vector2Int.zero);
    }

    public GridTile GetTile(int col, int row)
    {
        int index = (row * numColumns) + col;
        return tiles[index];
    }

    public Vector2Int ClampPoint(Vector2Int point)
    {
        int x = point.x;
        int y = point.y;

        y = Mathf.Clamp(y, 0, numRows - 1);
        x = Mathf.Clamp(x, 0, numColumns - 1);

        return new Vector2Int(x, y);
    }
}
