using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Fire")]
public class FireCard : CardType
{
    public override void UseCard(GameObject obj)
    {
        obj.GetComponent<TileObjBase>().Fire();
        obj.GetComponent<TileObjBase>().Move(-1);
    }
}
