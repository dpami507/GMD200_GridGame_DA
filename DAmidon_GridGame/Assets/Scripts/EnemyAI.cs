using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] CardHolder cardHolder;
    [SerializeField] GridManager gridManager;
    [SerializeField] CardList possibleCards;

    [SerializeField] List<CardScript> handCards;
    [SerializeField] List<CardScript> selectedCards;

    [SerializeField] PlayerScript player;
    [SerializeField] TileObjBase thisObj;

    bool running;

    private void Start()
    {
        running = false;

        player = FindFirstObjectByType<PlayerScript>();
        thisObj = GetComponent<TileObjBase>();

        cardHolder = FindFirstObjectByType<CardHolder>();
        cardHolder.AddRandomCards(selectedCards, handCards, this.transform, false);
    }

    public void StartTurn()
    {
        StartCoroutine(cardHolder.DestroyCards(selectedCards));
        //StartCoroutine(cardHolder.DestroyCards(handCards));
        cardHolder.AddRandomCards(selectedCards, handCards, this.transform, false);
        StartCoroutine(RunEnemyAI());
    }

    public IEnumerator RunEnemyAI()
    {
        running = true;

        while (running && selectedCards.Count + handCards.Count > 0)
        {
            yield return new WaitForSeconds(1f);

            //On equal x cords
            if (player.gridChords.x == thisObj.gridChords.x)
            {
                //If not facing turn
                if (DirToFace() != thisObj.dir)
                {
                    TryTurn();
                }
                else //Move or fire if Facing
                {
                    //If can fire fire
                    if (HasCard(CardType.Type.FIRE) >= 0)
                    {
                        int index = HasCard(CardType.Type.FIRE);
                        MoveCard(index);
                    }
                    //Else if it can move move
                    else if (HasCard(CardType.Type.MOVE) >= 0)
                    {
                        int index = HasCard(CardType.Type.MOVE);
                        MoveCard(index);
                    }
                    //else end turn
                    else
                    {
                        running = false;
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
            //If not on same x
            else if (player.gridChords.x != thisObj.gridChords.x)
            {
                //Check if facing correct way
                if (DirToFace() == thisObj.dir)
                {
                    //Try move if facing
                    TryMove();
                }
                //If not on same y
                else if (player.gridChords.y != thisObj.gridChords.y)
                {
                    //Are we facing correctly
                    if (DirToFace() == thisObj.dir)
                    {
                        //Try move if so
                        TryMove();
                    }
                    else
                    {
                        //Turn if not
                        TryTurn();
                    }
                }
                else
                {
                    TryTurn();
                }
            }
            else if (player.gridChords.y != thisObj.gridChords.y)
            {
                //Check if facing correct way
                if (DirToFace() == thisObj.dir)
                {
                    //Try move if facing
                    TryMove();
                }
                //If not on same y
                else if (player.gridChords.x != thisObj.gridChords.x)
                {
                    //Are we facing correctly
                    if (DirToFace() == thisObj.dir)
                    {
                        //Try move if so
                        TryMove();
                    }
                    else
                    {
                        //Turn if not
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
                TryMove();
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
                TryMove();
                Debug.Log("No TURNRIGHT Card Available");
                running = false;
            }
        }
        else
        {
            //Just turn in a direction
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
                TryMove();
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
        //If x dis < y dist
        if(Mathf.Abs(player.gridChords.x - thisObj.gridChords.x) < Mathf.Abs(player.gridChords.y - thisObj.gridChords.y))
        {
            //If to the left face left
            if (player.gridChords.x < thisObj.gridChords.x)
                return 180;
            //if to the right face right
            else if (player.gridChords.x > thisObj.gridChords.x)
                return 0;
            else //When x is ==
                //If below face down
                if (player.gridChords.y < thisObj.gridChords.y)
                    return 270;
                //face up
                else
                    return 90;
        }
        else //If y dist is less than x dist
        {
            //If below face down
            if (player.gridChords.y < thisObj.gridChords.y)
            {
                Debug.Log("Player Below");
                return 270;
            }
            //if above face up
            else if (player.gridChords.y > thisObj.gridChords.y)
            {
                Debug.Log("Player Above");
                return 90;
            }
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