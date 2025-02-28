using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : TileObjBase
{
    public GameObject explosionPrefab;
    [SerializeField] Transform muzzle;

    public override void Fire()
    {
        GameObject _explosion = Instantiate(explosionPrefab, muzzle.position, Quaternion.identity);
        _explosion.transform.localScale = Vector2.one * gridManager.squareSize;
        Destroy(_explosion, 1f);
    }
}
