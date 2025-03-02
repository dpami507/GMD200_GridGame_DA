using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjBase : MonoBehaviour
{
    public Vector2Int dirVector = Vector2Int.zero;
    public GridManager gridManager;
    public Vector2Int gridChords;

    public int dir = 0;

    private void Start()
    {
        dirVector = new Vector2Int((int)Mathf.Cos(dir * Mathf.Deg2Rad), (int)Mathf.Sin(dir * Mathf.Deg2Rad));
    }

    private void Update()
    {
        if (dir >= 360)
        {
            dir -= 360;
        }

        float t = 7 * Time.deltaTime;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, dir), t);
        transform.localPosition = Vector2.Lerp(transform.localPosition, gridManager.GetTile(gridChords.x, gridChords.y).transform.localPosition, t);
    }

    public void Move(int spaces)
    {
        Vector2Int pos;

        if(gridManager.enemy.gridChords == gridChords + (dirVector * spaces) || gridManager.player.gridChords == gridChords + (dirVector * spaces))
            pos = gridChords + (dirVector * (spaces - 1));
        else pos = gridChords + (dirVector * spaces);

        gridChords = gridManager.ClampPoint(pos);
    }

    public void Rotate(int deg)
    {
        dir += deg;
        dirVector = new Vector2Int((int)Mathf.Cos(dir * Mathf.Deg2Rad), (int)Mathf.Sin(dir * Mathf.Deg2Rad));
        Debug.Log($"{dir}, {dirVector}");
    }

    public virtual void Fire()
    {
        Debug.Log("Base Grid tile cannot fire");
    }
}
