using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Forward")]
public class ForwardCard : CardType
{
    public int spacesForward;

    public override void UseCard(GameObject player)
    {
        Debug.Log($"Moving {spacesForward} spaces!");
    }
}
