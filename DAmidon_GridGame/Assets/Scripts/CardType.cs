using UnityEngine;

public abstract class CardType : ScriptableObject
{
    public enum Type
    {
        FIRE, MOVE, TURNLEFT, TURNRIGHT
    }

    public Type type;

    public abstract void UseCard(GameObject obj);
}
