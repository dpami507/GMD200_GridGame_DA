using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVisuals : MonoBehaviour
{
    CardHolder cardHolder;
    public List<GameObject> points;
    public GameObject point;
    public Transform pointsParent;

    public TileObjBase ghostPlayer;

    void Start()
    {
        cardHolder = GetComponent<CardHolder>();
    }

    private void Update()
    {
        if(cardHolder.selectedCards.Count > 0)
        {
            if(!cardHolder.running)
                ShowFutureMoves();

            ghostPlayer.gameObject.SetActive(true);
        }
        else ghostPlayer.gameObject.SetActive(false);
    }

    void ShowFutureMoves()
    {
        ghostPlayer.gridChords = GridManager.instance.player.gridChords;
        ghostPlayer.dirVector = GridManager.instance.player.dirVector;
        ghostPlayer.dir = GridManager.instance.player.dir;

        for (int i = 0; i < cardHolder.selectedCards.Count; i++)
        {
            cardHolder.selectedCards[i].type.UseCard(ghostPlayer.gameObject);
        }
    }
}
