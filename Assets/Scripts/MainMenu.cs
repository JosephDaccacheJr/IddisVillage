using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject panelHowToPlay;
    public GameObject startFadeOut;
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        startFadeOut.SetActive(true);
        Invoke("GoToGame", 1.3f);
    }

    void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenHowToPlay()
    {
        panelHowToPlay.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        panelHowToPlay.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }
}
