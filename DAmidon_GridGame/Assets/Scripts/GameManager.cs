using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayersTurn;
    [SerializeField] bool setUp;
    public bool inPlay;
    [SerializeField] CardHolder cardHolder;
    [SerializeField] GameObject[] heartsPlayer;
    PlayerScript player;

    [Header("Enemy")]
    [SerializeField] GameObject[] heartsEnemy;
    EnemyAI enemyAI;

    public static GameManager instance;
    [SerializeField] GameObject endScreen;
    [SerializeField] TMP_Text winStateText;
    [SerializeField] string gameScene;
    [SerializeField] string menuScene;

    [SerializeField] GameObject EnemyTurnIndicator;
    [SerializeField] GameObject PlayerTurnIndicator;

    private void Awake()
    {
        instance = this;
        inPlay = true;
        isPlayersTurn = true;
    }

    private void Start()
    {
        EnemyTurnIndicator.SetActive(false);
        PlayerTurnIndicator.SetActive(false);

        endScreen.SetActive(false);
        enemyAI = GridManager.instance.enemy.GetComponent<EnemyAI>();
        player = GridManager.instance.player;
    }

    private void Update()
    {
        SetHearts(player.GetComponent<Health>().health, heartsPlayer);
        SetHearts(enemyAI.GetComponent<Health>().health, heartsEnemy);
        if (!inPlay)
        {
            if(!endScreen.activeInHierarchy)
                endScreen.SetActive(true);

            if (player.GetComponent<Health>().dead)
                winStateText.text = "Game Over";
            else
                winStateText.text = "You Win!";
            return;
        }

        if (isPlayersTurn && !setUp)
        {
            EnemyTurnIndicator.SetActive(false);
            PlayerTurnIndicator.SetActive(true);
            setUp = true;
            cardHolder.StartTurn();
        }
        else if (!isPlayersTurn && !setUp)
        {
            EnemyTurnIndicator.SetActive(true);
            PlayerTurnIndicator.SetActive(false);
            setUp = true;
            enemyAI.StartTurn();
        }
    }

    void SetHearts(int heartsActive, GameObject[] hearts)
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

    public void PlayAgain()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(menuScene);
    }
}
