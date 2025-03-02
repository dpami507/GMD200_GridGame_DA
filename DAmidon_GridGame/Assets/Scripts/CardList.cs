using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/List")]
public class CardList : ScriptableObject
{
    public CardScript[] cards;
}
