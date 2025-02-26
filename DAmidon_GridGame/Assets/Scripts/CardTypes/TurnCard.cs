using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Turn")]
public class TurnCard : CardType
{
    public int degrees;

    public override void UseCard(GameObject player)
    {
        if(degrees == 90)
            Debug.Log($"Turning Right {degrees} degrees!");
        else if(degrees == -90)
            Debug.Log($"Turning Left {degrees} degrees!");
        else
            Debug.Log($"Cant Turn {degrees} degrees!");
    }
}
