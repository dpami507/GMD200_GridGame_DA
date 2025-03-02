using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public CardHolder cardHolder;
    public GridManager gridManager;
    public CardList possibleCards;

    public List<CardScript> handCards;
    public List<CardScript> selectedCards;

    public PlayerScript player;
    public TileObjBase thisObj;

    bool running;

    private void Start()
    {
        running = false;

        player = FindFirstObjectByType<PlayerScript>();
        thisObj = GetComponent<TileObjBase>();

        cardHolder = FindFirstObjectByType<CardHolder>();
        cardHolder.AddRandomCards(selectedCards, handCards, this.transform, false);
    }

    public IEnumerator RunEnemyAI()
    {
        running = true;

        while (running)
        {
            yield return new WaitForSeconds(1);

            //On equal x cords
            if (player.gridChords.x == thisObj.gridChords.x)
            {
                //If not facing turn
                if (DirToFace() != thisObj.dir)
                {
                    TryTurn();
                }
                else //Move or fire
                {
                    if (HasCard(CardType.Type.FIRE) >= 0)
                    {
                        int index = HasCard(CardType.Type.FIRE);
                        MoveCard(index);
                    }
                    if (HasCard(CardType.Type.MOVE) >= 0)
                    {
                        int index = HasCard(CardType.Type.MOVE);
                        MoveCard(index);
                    }
                }
            }
            //On equal y cords
            else if (player.gridChords.y == thisObj.gridChords.y)
            {
                //If not facing turn
                if (DirToFace() != thisObj.dir)
                {
                    TryTurn();
                }
                else //Move or fire
                {
                    if (HasCard(CardType.Type.FIRE) >= 0)
                    {
                        int index = HasCard(CardType.Type.FIRE);
                        MoveCard(index);
                    }
                    else
                    {
                        Debug.Log("No FIRE Card Available");
                        TryMove();
                    }
                }
            }
            else if (player.gridChords.x != thisObj.gridChords.x)
            {
                if (DirToFace() == thisObj.dir)
                {
                    TryMove();
                }
                else if (player.gridChords.y != thisObj.gridChords.y)
                {
                    if (DirToFace() == thisObj.dir)
                    {
                        TryMove();
                    }
                    else
                    {
                        TryTurn();
                    }
                }
                else
                {
                    TryTurn();
                }
            }

            for (int i = 0; i < selectedCards.Count; i++)
            {
                if (selectedCards[i] == null)
                {
                    selectedCards.RemoveAt(i);
                }
            }

            for (int i = 0; i < selectedCards.Count; i++)
            {
                if (selectedCards[i] == null)
                {
                    selectedCards.RemoveAt(i);
                }
                else
                {
                    selectedCards[i].type.UseCard(thisObj.gameObject);

                    Destroy(selectedCards[i].gameObject);
                }
            }
        }

        GameManager.instance.EndTurn(true);
    }

    void TryTurn()
    {
        if (thisObj.dir - DirToFace() == -90)
        {
            if (HasCard(CardType.Type.TURNLEFT) >= 0)
            {
                int index = HasCard(CardType.Type.TURNLEFT);
                MoveCard(index);
            }
            else
            {
                Debug.Log("No TURNLEFT Card Available");
                running = false;
            }
        }
        else if (thisObj.dir - DirToFace() == 90)
        {
            if (HasCard(CardType.Type.TURNRIGHT) >= 0)
            {
                int index = HasCard(CardType.Type.TURNRIGHT);
                MoveCard(index);
            }
            else
            {
                Debug.Log("No TURNRIGHT Card Available");
                running = false;
            }
        }
        else
        {
            if (HasCard(CardType.Type.TURNRIGHT) >= 0)
            {
                int index = HasCard(CardType.Type.TURNRIGHT);
                MoveCard(index);
            }
            else if (HasCard(CardType.Type.TURNLEFT) >= 0)
            {
                int index = HasCard(CardType.Type.TURNLEFT);
                MoveCard(index);
            }
            else
            {
                Debug.Log("No TURN Card Available");
                running = false;
            }
        }
    }

    void TryMove()
    {
        if (HasCard(CardType.Type.MOVE) >= 0)
        {
            int index = HasCard(CardType.Type.MOVE);
            MoveCard(index);
        }
        else
        {
            Debug.Log("No MOVE Card Available");
            running = false;
        }
    }

    void MoveCard(int index)
    {
        CardScript card = handCards[index];
        selectedCards.Add(card);
        handCards.Remove(card);
    }

    float DirToFace()
    {
        //Driection is the negative normalized of the value
        if(Mathf.Abs(player.gridChords.x - thisObj.gridChords.x) < Mathf.Abs(player.gridChords.y - thisObj.gridChords.y))
        {
            if (player.gridChords.x < thisObj.gridChords.x)
                return 180;
            else if (player.gridChords.x < thisObj.gridChords.x)
                return 0;
            else
            {
                if (player.gridChords.y < thisObj.gridChords.y)
                    return 270;
                else
                    return 90;
            }
        }
        else
        {
            if (player.gridChords.y < thisObj.gridChords.y)
                return 270;
            else if (player.gridChords.y > thisObj.gridChords.y)
                return 90;
            else //when y is ==
            {
                if (player.gridChords.x < thisObj.gridChords.x)
                    return 180;
                else
                    return 0;
            }
        }
    }

    int HasCard(CardType.Type type)
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i].type.type == type)
                return i;
        }

        return -1;
    }
}