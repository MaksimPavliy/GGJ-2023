using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlaying;
    public List<Level> levels;

    public static GameManager instance;
    public GameMode gameMode = GameMode.StoryMode;
    public NetworkMode networkMode = NetworkMode.Local;

    private int collectedRootsAmount;

    public static UnityAction OnCollectedCounterUpdated;
    public static UnityAction OnWin;
    public static UnityAction OnLose;
    public int currentLevelId = 3;
    [HideInInspector] public Level curLevel;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        LoadMainMenu();
    }

    public void UpdateRootCounter()
    {
        if (collectedRootsAmount < curLevel.requiredRootsAmount)
        {
            OnCollectedCounterUpdated?.Invoke();
        }
        collectedRootsAmount++;

        if (collectedRootsAmount >= curLevel.requiredRootsAmount)
        {
            isPlaying = false;
            WinGame();
        }
    }

    public void LoadLevel()
    {
        if (curLevel)
            Destroy(curLevel.gameObject);

        if (currentLevelId < levels.Count)
        {
            collectedRootsAmount = 0;
            curLevel = Instantiate(levels.Find(x => x.levelId == currentLevelId));
            Time.timeScale = 1;
            isPlaying = true;
        }
    }

    public void LoadMainMenu()
    {
        currentLevelId = 2;
        LoadLevel();
    }

    public void WinGame()
    {
        isPlaying = false;
        currentLevelId++;
        DOTween.KillAll();
        LoadLevel();
    }

    public void LoseGame()
    {
        isPlaying = false;
        DOTween.KillAll();
        LoadLevel();
    }

    private void SetGameMode(GameMode gameMode)
    {
        this.gameMode = gameMode;
    }

    private void SetNetworkMode(NetworkMode networkMode)
    {
        this.networkMode = networkMode;
    }

    public enum GameMode
    {
        StoryMode,
        Versus
    }
    public enum NetworkMode
    {
        Local,
        Multiplayer
    }
}
