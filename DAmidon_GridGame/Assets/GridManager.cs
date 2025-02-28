using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int numRows;
    public int numColumns;

    public float padding;
    public float squareSize;

    [SerializeField] GridTile tilePrefab;

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
                GridTile tile = Instantiate(tilePrefab, transform);

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

        transform.position = new Vector2(x, y);
    }

    void CreatePlayer()
    {
        player = Instantiate(playerPrefab, transform).GetComponent<PlayerScript>();
        player.transform.localScale = Vector3.one * squareSize;
        player.gridManager = this;
        SetPoint(Vector2Int.zero);
    }

    public Vector2Int SetPoint(Vector2Int point)
    {
        int x = point.x;
        int y = point.y;

        y = Mathf.Clamp(y, 0, numRows - 1);
        x = Mathf.Clamp(x, 0, numColumns - 1);

        return new Vector2Int(x, y);
    }

    public Vector2 PointToWorld(Vector2Int point)
    {
        Vector2 worldPos = new Vector2((point.x * squareSize) + (point.x * padding), (point.y * squareSize) + (point.y * padding));
        return worldPos;
    }
}
