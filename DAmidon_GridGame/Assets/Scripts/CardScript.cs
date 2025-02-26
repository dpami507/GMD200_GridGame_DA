using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public CardType type;

    public bool hovered;

    void OnMouseOver()
    {
        hovered = true;
    }
    private void OnMouseExit()
    {
        hovered = false;
    }
}
