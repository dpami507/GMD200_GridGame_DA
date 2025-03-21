using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] int cardMoveSpeed;
    public CardType type;

    public bool hovered;
    public Vector2 pos;

    [SerializeField] float shrinkSpeed;
    [HideInInspector] public bool destroyed;
    float destroyedMag = 3;
    float spinSpeed = 360;

    private void Update()
    {
        //Lerp position to pos
        transform.localPosition = Vector2.Lerp(transform.localPosition, pos, cardMoveSpeed * Time.deltaTime);

        if(destroyed)
        {
            //Lerp scale until small enough to destroy
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
