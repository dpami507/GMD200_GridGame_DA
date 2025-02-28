using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class CardHolder : MonoBehaviour
{
    [Header("Card Lists")]
    public List<CardScript> cards;
    public List<CardScript> selectedCards;

    [Header("Cards Setup")]
    public CardList possibleCards;
    public GameObject cardOutline;
    public int maxCards;

    [Header("Cards Positioning")]
    public Transform handTrans;
    public Transform selectedTrans;
    public float cardSpacing;
    public float hoverHeight;
    public float cardMoveSpeed;

    [Header("Cards Running")]
    public Transform indicatorArrow;
    public float indicatorYOffset;
    public float indicatorSpeed;
    Vector2 indicatorPos;

    public bool running;

    private void Start()
    {
        indicatorArrow.gameObject.SetActive(false);
        AddOutlineVisuals();
        running = false;
    }

    private void Update()
    {
        //If cards are running dont allow any input or anything
        if (!running)
        {
            if (Input.GetKeyDown(KeyCode.H)) //Fills with random cards
                AddRandomCards();
            else if (Input.GetKeyDown(KeyCode.R)) //Clears all cards in lists
                ClearCards();
            else if (Input.GetKeyDown(KeyCode.Space)) //Run cards
                StartCoroutine(RunCards());

            if (Input.GetMouseButtonDown(0)) //Add Card or Remove card to 'selected cards'
            {
                Collider2D clickedCard = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //Pointcast

                if (clickedCard && clickedCard.GetComponent<CardScript>()) //If we actually click a card
                {
                    //Swap
                    CardScript card = clickedCard.GetComponent<CardScript>();

                    if(!card.destroyed)
                    {
                        if (selectedCards.Contains(card))
                        {
                            cards.Add(card);
                            card.transform.parent = handTrans;
                            selectedCards.Remove(card);
                        }
                        else
                        {
                            cards.Remove(card);
                            card.transform.parent = selectedTrans;
                            selectedCards.Add(card);
                        }
                    }
                }
            }
        }

        //Format and position selected cards
        UpdateCardPositions(selectedCards);

        //Format and position hand cards
        UpdateCardPositions(cards);

        //Lerp Arrow Speed
        indicatorArrow.localPosition = Vector2.Lerp(indicatorArrow.localPosition, indicatorPos, indicatorSpeed * Time.deltaTime);
    }

    void UpdateCardPositions(List<CardScript> cardsList)
    {
        for (int i = 0; i < cardsList.Count; i++)
        {
            if (cardsList[i] == null)
            {
                cardsList.Remove(cardsList[i]);
                return;
            }

            if (running) return;
            float yOff = (cardsList[i].hovered) ? hoverHeight : 0;
            cardsList[i].pos = new Vector2(cardSpacing * i, yOff);
        }
    }

    //Formats card outlines
    void AddOutlineVisuals()
    {
        for (int i = 0; i < maxCards; i++)
        {
            GameObject _card = Instantiate(cardOutline, handTrans);
            _card.transform.localPosition = new Vector2(cardSpacing * i, 0);

            _card = Instantiate(cardOutline, selectedTrans);
            _card.transform.localPosition = new Vector2(cardSpacing * i, 0);
        }
    }

    //Removes all cards
    void ClearCards()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i].gameObject);
        }
        for (int i = 0; i < selectedCards.Count; i++)
        {
            Destroy(selectedCards[i].gameObject);
        }
    }

    //Fills with random cards
    void AddRandomCards()
    {
        ClearCards();

        for (int i = 0; i < maxCards; i++)
        {
            GameObject _card = Instantiate(possibleCards.cards[Random.Range(0, possibleCards.cards.Length)], handTrans);
            _card.GetComponent<CardScript>().holder = this;
            cards.Add(_card.GetComponent<CardScript>());
        }
    }

    //Runs the cards
    IEnumerator RunCards()
    {
        running = true;
        indicatorArrow.gameObject.SetActive(true);

        //Go through both sets and set their position to nonhover pos
        for (int i = 0; i < selectedCards.Count; i++)
        {
            selectedCards[i].pos = new Vector2(cardSpacing * i, 0);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].pos = new Vector2(cardSpacing * i, 0);
        }

        //Go through and run each card
        for (int i = 0; i < selectedCards.Count; i++) 
        {
            Vector2 pos = new Vector2(selectedCards[i].transform.localPosition.x, hoverHeight);
            indicatorPos = new Vector2(cardSpacing * i, indicatorYOffset);

            selectedCards[i].pos = pos;
            selectedCards[i].type.UseCard(GridManager.instance.player.gameObject);
            yield return new WaitForSeconds(1);
        }

        indicatorPos = new Vector2(cardSpacing * (selectedCards.Count - 1), 0);

        //loop through and put new position
        for (int i = 0; i < selectedCards.Count; i++)
        {
            Vector2 pos = new Vector2(selectedCards[i].transform.localPosition.x, 0);

            selectedCards[i].pos = pos;
            yield return new WaitForSeconds(.1f); //offset for cool effect
        }

        indicatorArrow.gameObject.SetActive(false);

        //Destroy all the cards (clean up logic is in Update)
        StartCoroutine(DestroyCards());
    }

    IEnumerator DestroyCards()
    {
        //Create new Array to counteract the cleanup
        CardScript[] cardsToDelete = selectedCards.ToArray();

        //loop through and destroy them
        for (int i = 0; i < cardsToDelete.Length; i++)
        {
            cardsToDelete[i].destroyed = true;
            yield return new WaitForSeconds(.1f); //offset for cool effect
        }

        yield return new WaitForSeconds(.5f);

        running = false;
    }
}
