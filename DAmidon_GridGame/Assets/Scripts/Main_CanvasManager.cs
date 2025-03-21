using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Main_CanvasManager : MonoBehaviour
{
    [SerializeField] string gameScene;
    [SerializeField] GameObject howToPlay;

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
