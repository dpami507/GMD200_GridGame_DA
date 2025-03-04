using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Turn")]
public class TurnCard : CardType
{
    public int degrees;

    public override void UseCard(GameObject obj)
    {
        obj.GetComponent<TileObjBase>().Rotate(degrees);
    }
}
