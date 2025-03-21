using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVisuals : MonoBehaviour
{
    [SerializeField] CardHolder cardHolder;
    GridManager gridManager;
    [SerializeField] List<GameObject> points;
    [SerializeField] GameObject point;
    [SerializeField] Transform pointsParent;

    [SerializeField] TileObjBase ghostPlayer;

    int lastFutureSelectedCards;
    LineRenderer lr;

    void Start()
    {
        gridManager = GetComponent<GridManager>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1;
    }

    private void Update()
    {
        //Show future pos if cards are not running
        if (!cardHolder.running)
        {
            if(lastFutureSelectedCards != cardHolder.selectedCards.Count)
            {
                ShowFutureMoves();
            }
            lr.SetPosition(cardHolder.selectedCards.Count, RoundVector(ghostPlayer.transform.position)); //Set last pos
        }

        //Set invisible if no cards are being played
        if(cardHolder.selectedCards.Count == 0)
        {
            ghostPlayer.transform.position = gridManager.player.transform.position;
            ClearPointsList();
            ghostPlayer.gameObject.SetActive(false);
        }
        else if (cardHolder.selectedCards.Count > 0) //Show lr and arrow if cards are selected
        {
            lr.enabled = true;
            ghostPlayer.gameObject.SetActive(true);
        }
        else
        {
            lr.enabled = false;
            ClearPointsList();
            ghostPlayer.gameObject.SetActive(false);
        }
    }

    void ShowFutureMoves()
    {
        ClearPointsList(); //Clears the points
        lastFutureSelectedCards = cardHolder.selectedCards.Count; //Sets the last known amount of cards

        ghostPlayer.gridChords = gridManager.player.gridChords; //Set ghost player to player cords
        ghostPlayer.dirVector = gridManager.player.dirVector; //Set ghost dirVector to player
        ghostPlayer.dir = gridManager.player.dir; //Set ghost dir to player

        //For all the cards in the selected cards do that on the ghost player
        for (int i = 0; i < cardHolder.selectedCards.Count; i++)
        {
            Vector2 pointPos = RoundVector((Vector2)gridManager.GetTile(ghostPlayer.gridChords.x , ghostPlayer.gridChords.y).transform.localPosition + gridManager.gridOffset);

            lr.positionCount++; //Create new position for lr
            cardHolder.selectedCards[i].type.UseCard(ghostPlayer.gameObject);

            GameObject _point = Instantiate(point, pointPos, Quaternion.identity, pointsParent);
            points.Add(_point);

            //Set all positions
            lr.SetPosition(i, pointPos);
        }
    }

    Vector2 RoundVector(Vector2 vec)
    {
        float x = vec.x;
        x *= 100f;
        x = Mathf.RoundToInt(x);
        x /= 100f;

        float y = vec.y;
        y *= 100f;
        y = Mathf.RoundToInt(y);
        y /= 100f;

        Vector2 result = new Vector2(x, y);

        return result;
    }

    void ClearPointsList()
    {
        //Get amount of points
        int pointsCount = points.Count;

        //Kill them all
        for (int i = 0; i < pointsCount; i++)
        {
            Destroy(points[i]);
        }

        //Clear the list
        points.Clear();

        //Set LR to 0
        lr.positionCount = 1;
    }
}
