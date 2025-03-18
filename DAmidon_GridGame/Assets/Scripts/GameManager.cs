using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayersTurn;
    public bool setUp;
    public bool inPlay;
    public CardHolder cardHolder;
    public GameObject[] heartsPlayer;
    PlayerScript player;

    [Header("Enemy")]
    public GameObject[] heartsEnemy;
    EnemyAI enemyAI;

    public static GameManager instance;
    public GameObject endScreen;
    public TMP_Text winStateText;
    public string gameScene;
    public string menuScene;

    private void Awake()
    {
        instance = this;
        inPlay = true;
        isPlayersTurn = true;
    }

    private void Start()
    {
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
            setUp = true;
            cardHolder.StartTurn();
        }
        else if (!isPlayersTurn && !setUp)
        {
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
