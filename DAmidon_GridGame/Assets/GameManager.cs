using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayersTurn;
    public bool setUp;
    public CardHolder cardHolder;

    [Header("Enemy")]
    EnemyAI enemyAI;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enemyAI = GridManager.instance.enemy.GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (isPlayersTurn && !setUp)
        {
            setUp = true;
            cardHolder.StartTurn();
        }
        else if (!isPlayersTurn && !setUp)
        {
            setUp = true;
            enemyAI.StartTurn();
            StartCoroutine(enemyAI.RunEnemyAI());
        }
    }

    public void EndTurn(bool isTurn)
    {
        isPlayersTurn = isTurn;
        setUp = false;
    }
}
