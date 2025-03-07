using DG.Tweening;
using UnityEngine;

public class TileObjBase : MonoBehaviour
{
    public Vector2Int dirVector = Vector2Int.zero;
    public GridManager gridManager;
    public Vector2Int gridChords;

    public float moveDuration;
    public Ease ease;
    public int dir = 0;

    private void Start()
    {
        dirVector = new Vector2Int((int)Mathf.Cos(dir * Mathf.Deg2Rad), (int)Mathf.Sin(dir * Mathf.Deg2Rad));
        transform.localRotation = Quaternion.Euler(0, 0, dir);
    }

    private void Update()
    {
        if (dir >= 360)
        {
            dir -= 360;
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, dir), 7 * Time.deltaTime);
    }

    public void Move(int spaces)
    {
        Vector2Int pos = TrySpace(spaces);

        gridChords = gridManager.ClampPoint(pos);
        transform.DOMove(gridManager.GetTile(gridChords.x, gridChords.y).transform.position, moveDuration).SetEase(ease);
    }
    Vector2Int TrySpace(int spaces)
    {
        Vector2Int pos = gridChords + (dirVector * spaces);

        if (gridManager.enemy.gridChords == pos || gridManager.player.gridChords == pos)
            pos = TrySpace(spaces - 1);
        else
            pos = gridChords + (dirVector * (spaces));

        return pos;
    }

    public void Rotate(int deg)
    {
        dir += deg;
        dirVector = new Vector2Int((int)Mathf.Cos(dir * Mathf.Deg2Rad), (int)Mathf.Sin(dir * Mathf.Deg2Rad));
    }

    public virtual void Fire()
    {
        Debug.Log("Base Grid tile cannot fire (most likely called on ghost player)");
    }
}
