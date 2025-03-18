using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Main_CanvasManager : MonoBehaviour
{
    public string gameScene;
    public GameObject howToPlay;

    private void Start()
    {
        howToPlay.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleHowToPlay()
    {
        howToPlay.SetActive(!howToPlay.activeInHierarchy);
    }
}
