using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVisuals : MonoBehaviour
{
    public CardHolder cardHolder;
    GridManager gridManager;
    public List<GameObject> points;
    public GameObject point;
    public Transform pointsParent;

    public TileObjBase ghostPlayer;

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
        if (cardHolder.selectedCards.Count > 0)
        {
            if (!cardHolder.running && cardHolder.selectedCards.Count != lastFutureSelectedCards)
            ShowFutureMoves();

            lr.SetPosition(lr.positionCount - 1, (Vector2)ghostPlayer.transform.localPosition + gridManager.gridOffset);
            lr.SetPosition(0, gridManager.PointToWorld(gridManager.player.gridChords) + gridManager.gridOffset);

            lr.enabled = true;
            ghostPlayer.gameObject.SetActive(true);
        }
        else
        {
            lr.enabled = false;
            ghostPlayer.transform.localPosition = gridManager.player.transform.localPosition;
            ghostPlayer.gameObject.SetActive(false);
        }
    }

    void ShowFutureMoves()
    {
        lastFutureSelectedCards = cardHolder.selectedCards.Count;
        ClearPointsList();

        ghostPlayer.gridChords = gridManager.player.gridChords;
        ghostPlayer.dirVector = gridManager.player.dirVector;
        ghostPlayer.dir = gridManager.player.dir;

        for (int i = 0; i < cardHolder.selectedCards.Count; i++)
        {
            lr.positionCount++;
            cardHolder.selectedCards[i].type.UseCard(ghostPlayer.gameObject);

            Vector2 pointPos = gridManager.PointToWorld(ghostPlayer.gridChords)
                + gridManager.gridOffset;

            GameObject _point = Instantiate(point, pointPos, Quaternion.identity, pointsParent);
            points.Add(_point);

            if (i != cardHolder.selectedCards.Count - 1)
                lr.SetPosition(i + 1, pointPos);
        }
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
