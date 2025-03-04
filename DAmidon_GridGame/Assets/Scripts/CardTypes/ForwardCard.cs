using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Forward")]
public class ForwardCard : CardType
{
    public int spacesForward;

    public override void UseCard(GameObject obj)
    {
        obj.GetComponent<TileObjBase>().Move(spacesForward);
    }
}
