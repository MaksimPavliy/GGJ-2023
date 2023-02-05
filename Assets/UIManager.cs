using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Canvas tutorialCanvas;
    public Canvas loseCanvas;
    public Canvas winCanvas;
    public Canvas pauseCanvas;

    public static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.OnWin += ShowWinCanvas;
        GameManager.OnLose += ShowLoseCanvas;
    }

    private void Pause()
    {
        GameManager.instance.isPlaying = false;
        pauseCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void Unpause()
    {
        GameManager.instance.isPlaying = true;
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void ShowWinCanvas()
    {
        winCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void ShowLoseCanvas()
    {
        loseCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void LoadNextLevel()
    {
        GameManager.instance.LoadLevel();
        Time.timeScale = 1;
    }
}
