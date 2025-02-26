using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CardHolder : MonoBehaviour
{
    [Header("Card Lists")]
    public List<CardScript> cards;
    public List<CardScript> selectedCards;

    [Header("Cards Setup")]
    public CardList possibleCards;
    public GameObject cardOutline;
    public Transform outlineParent;
    public int maxCards;

    [Header("Cards Positioning")]
    public float cardSpacing;
    public float hoverHeight;
    public float selectedCardsYOffset;

    [Header("Cards Running")]
    public Transform indicatorArrow;
    public float indicatorYOffset;

    //Private
    float yPos;
    bool running;

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
            if (Input.GetKeyDown(KeyCode.A)) //Fills with random cards
                AddRandomCards();
            else if (Input.GetKeyDown(KeyCode.R)) //Clears all cards in lists
                ClearCards();
            else if (Input.GetKeyDown(KeyCode.Space)) //Run cards
                StartCoroutine(RunCards());

            if (Input.GetMouseButtonDown(0)) //Add Card or Remove card to 'selected cards'
            {
                Collider2D clickedCard = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //Pointcast

                if (clickedCard != null) //If we actually click a card
                {
                    //Swap
                    CardScript card = clickedCard.GetComponent<CardScript>();
                    if (selectedCards.Contains(card))
                    {
                        cards.Add(card);
                        selectedCards.Remove(card);
                    }
                    else
                    {
                        cards.Remove(card);
                        selectedCards.Add(card);
                    }
                }
            }
        }

        //Format and position selected cards
        for (int i = 0; i < selectedCards.Count; i++)
        {
            if (selectedCards[i] == null)
            {
                selectedCards.Remove(selectedCards[i]);
                return;
            }

            yPos = (selectedCards[i].hovered) ? hoverHeight : 0;
            selectedCards[i].transform.localPosition = new Vector2(cardSpacing * i, yPos + selectedCardsYOffset);
        }

        //Format and position hand cards
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == null)
            {
                cards.Remove(cards[i]);
                return;
            }

            yPos = (cards[i].hovered) ? hoverHeight : 0;
            cards[i].transform.localPosition = new Vector2(cardSpacing * i, yPos);
        }
    }

    //Formats card outlines
    void AddOutlineVisuals()
    {
        for (int i = 0; i < maxCards; i++)
        {
            GameObject _card = Instantiate(cardOutline, outlineParent);
            _card.transform.localPosition = new Vector2(cardSpacing * i, 0);

            _card = Instantiate(cardOutline, outlineParent);
            _card.transform.localPosition = new Vector2(cardSpacing * i, selectedCardsYOffset);
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
            GameObject _card = Instantiate(possibleCards.cards[Random.Range(0, possibleCards.cards.Length)], transform);
            cards.Add(_card.GetComponent<CardScript>());
        }
    }

    //Runs the cards
    IEnumerator RunCards()
    {
        running = true;
        indicatorArrow.gameObject.SetActive(true);

        //Go through and run each card
        for (int i = 0; i < selectedCards.Count; i++) 
        {
            indicatorArrow.localPosition = new Vector2(cardSpacing * i, indicatorYOffset);
            selectedCards[i].type.UseCard(this.gameObject);
            yield return new WaitForSeconds(1);
        }

        //Destroy all the cards (clean up logic is in Update)
        for(int i = 0; i < selectedCards.Count; i++)
        {
            Destroy(selectedCards[i].gameObject);
        }

        indicatorArrow.gameObject.SetActive(false);
        running = false;
    }
}
