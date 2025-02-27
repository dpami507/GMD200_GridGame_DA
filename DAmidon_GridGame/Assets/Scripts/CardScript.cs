using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public CardHolder holder;
    public CardType type;

    public bool hovered;
    public Vector2 pos;

    public float shrinkSpeed;
    [HideInInspector] public bool destroyed;
    float destroyedMag = 3;
    float spinSpeed = 360;

    private void Update()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, pos, holder.cardMoveSpeed * Time.deltaTime);

        if(destroyed)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, shrinkSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);

            if(transform.localScale.magnitude < destroyedMag)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnMouseOver()
    {
        hovered = true;
    }
    private void OnMouseExit()
    {
        hovered = false;
    }
}
