using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : TileObjBase
{
    public GameObject explosionPrefab;
    [SerializeField] Transform muzzle;
    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public override void Fire()
    {
        GameObject _explosion = Instantiate(explosionPrefab, muzzle.position, Quaternion.identity);
        _explosion.transform.localScale = Vector2.one * gridManager.squareSize;
        Destroy(_explosion, 1f);


        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, muzzle.right, 100);
        if (hit && hit.transform.GetComponent<Health>())
        {
            Debug.Log("hit");
            hit.transform.GetComponent<Health>().TakeDamage();
        }
    }
}
