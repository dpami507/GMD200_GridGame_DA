using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayersTurn;
    public bool setUp;
    public bool inPlay;
    public CardHolder cardHolder;

    [Header("Enemy")]
    public GameObject[] hearts;
    EnemyAI enemyAI;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        inPlay = true;
        isPlayersTurn = true;
    }

    private void Start()
    {
        enemyAI = GridManager.instance.enemy.GetComponent<EnemyAI>();
    }

    private void Update()
    {
        SetHearts(enemyAI.GetComponent<Health>().health);
        if (!inPlay) return;

        if (isPlayersTurn && !setUp)
        {
            setUp = true;
            cardHolder.StartTurn();
        }
        else if (!isPlayersTurn && !setUp)
        {
            setUp = true;
            enemyAI.StartTurn();
        }
    }

    void SetHearts(int heartsActive)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < heartsActive)
                hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }
    }

    public void EndTurn(bool isTurn)
    {
        Debug.Log("Ending Turn");
        isPlayersTurn = isTurn;
        setUp = false;
    }
}
