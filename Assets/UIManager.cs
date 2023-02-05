using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject loseCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject mainMenuCanvas;


    private void Start()
    {
        if (tutorialCanvas)
        {
           ShowTutorialCanvas();
        }
    }

    public void LoadMainMenu()
    {
        GameManager.instance.LoadMainMenu();
    }

    public void OnPlayClicked()
    {
        mainMenuCanvas.SetActive(true);
        GameManager.instance.currentLevelId = 0;
        LoadNextLevel();
    }

    /*public void CloseGame()
    {
        Application.Quit();
    }*/

    public void ShowTutorialCanvas()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        tutorialCanvas.gameObject.SetActive(true);
    }

    public void HideTutorialCanvas()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        tutorialCanvas.SetActive(false);
    }

    /*public void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        GameManager.instance.isPlaying = false;
        pauseCanvas.SetActive(true);
    }*/

    /*public void Unpause()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        GameManager.instance.isPlaying = true;
        pauseCanvas.SetActive(true);
    }*/

    /*public void ShowWinCanvas()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        winCanvas.SetActive(true);
    }

    public void ShowLoseCanvas()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        loseCanvas.SetActive(true);
    }*/

    public void LoadNextLevel()
    {
        GameManager.instance.LoadLevel();
    }
}
